-- =============================================
-- Author:      Rafael Antunes
-- Create date: 15/07/2025 
-- Description: Função que valida CPF, retorna 1 = válido, 0 = inválido
-- =============================================
CREATE FUNCTION dbo.FI_FN_VerificaCpf
(
    @CPF VARCHAR(14)  -- aceita máscara com '.' e '-'
)
RETURNS BIT
AS
BEGIN
    DECLARE @CpfLimpo CHAR(11);
    DECLARE @Soma     INT;
    DECLARE @Resultado INT;
    DECLARE @i        INT;


    -- Não usei função para customização da mensagem de erro, por isso procedure
    
    IF @CPF IS NULL
        THROW 51005, 'CPF não pode ser nulo.', 1;

    -- Removo mascara do CPF mesmo que não tenha para prevenção
    SET @CpfLimpo = REPLACE(REPLACE(REPLACE(@CPF, '.', ''), '-', ''), ' ', '');

    -- Verifica comprimento da string resultante e verifica se não está usando números repetidos tipo 111.111.111-11
    IF LEN(@CpfLimpo) <> 11 OR @CpfLimpo LIKE REPLICATE(SUBSTRING(@CpfLimpo,1,1), 11)
    BEGIN
        RETURN 0
    END;

    -- Validação do primeiro dígito validador
    -- Criação de váriaveis locais para utilização
    SET @Soma = 0;
    SET @Resultado = 0;
    SET @i  = 1;

    -- Uso os 10 primeiros dígitos, porque os 2 últimos são os de validação
    WHILE @i <= 9
    BEGIN
        -- Essa é uma função otimizada, a ideia é somar o peso de uma cadeia de caracteres
        -- Exemplo:  1000000002 onde esse número '1' tem o peso 10 e o número '2' tem o peso 1
        -- então seria o somatório de cada uma dessas posições, multiplicado pelo peso dela
        -- Seria esse cálculo: 1×10+ 0×9+ 0x8+ 0x7+ 0x6+ 0x5+ 0x4+ 0x3+ 0×2+ 2×1 = 12  
        SET @Soma += CAST(SUBSTRING(@CpfLimpo, @i, 1) AS INT) * (11 - @i);

        -- Pulo para o próximo dígito
        SET @i += 1;
    END

    -- Uso o 11 e subtrario o resto da divisão por 11 desse somatório que fizemos
    SET @Resultado = 11 - (@Soma % 11);

    -- Se esse somatório der 10, o dígito validador é 0
    IF @Resultado >= 10 SET @Resultado = 0;

    -- Agora testo se o meu dígito calculado é o mesmo do cpf na posição 11
    IF @Resultado <> CAST(SUBSTRING(@CpfLimpo, 10, 1) AS INT)
    BEGIN
        RETURN 0
    END

    -- Validação do segundo dígito, é a mesma coisa do outro porém usando os 11 dígitos agora
    SET @Soma = 0; 
    SET @i = 1;

    WHILE @i <= 10
    BEGIN
        SET @Soma += CAST(SUBSTRING(@CpfLimpo, @i, 1) AS INT) * (12 - @i);
        SET @i += 1;
    END
    SET @Resultado = 11 - (@Soma % 11);

    -- Se esse somatório der 10, o dígito validador é 0
    IF @Resultado >= 10 SET @Resultado = 0;

    -- Agora testo se o meu dígito calculado é o mesmo do cpf na posição 12
    IF @Resultado <> CAST(SUBSTRING(@CpfLimpo, 11, 1) AS INT)
    BEGIN
        RETURN 0;
    END

    -- Retorna 1 para se tiver dado tudo certo
    RETURN 1;
END

