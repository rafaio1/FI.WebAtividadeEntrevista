-- =============================================
-- Author:      Rafael Antunes
-- Create date: 15/07/2025 20:15
-- Description: Valida CPF
-- =============================================
CREATE PROCEDURE dbo.FI_SP_VerificaCpf
    @CPF VARCHAR(14)  -- 14 para permitir ponto e hífen, se vier mascarado
AS
BEGIN
	-- Desabilito o NOCOUNT por ser uma procedure de função especifica (No Count da retorno da quantidade de linhas afetadas)
    -- Ativo o XACT_ABORT para qualquer erro de execução literalmente parar a execução, forçar tambem um ROLLBACK para tratativa de erro
    SET NOCOUNT, XACT_ABORT ON;

    IF @CPF IS NULL
        THROW 51005, 'CPF não pode ser nulo.', 1;

    -- Supondo um ambiente de produção da aplicação, onde terá mais inserts e updates usando essa procedure (não diretamente na tabela)
    -- Tambem vou criar triggers para exemplificação de como seria, se caso tivesse de usar trigger

    -- Removo mascara do CPF mesmo que não tenha para prevenção
    DECLARE @CpfLimpo CHAR(11) = REPLACE(REPLACE(REPLACE(@CPF, '.', ''), '-', ''), ' ', '');

    -- Verifica comprimento da string resultante e verifica se não está usando números repetidos tipo 111.111.111-11
    IF LEN(@CpfLimpo) <> 11 OR @CpfLimpo LIKE REPLICATE(SUBSTRING(@CpfLimpo,1,1), 11)
    BEGIN
        -- Deixei os códigos de erro parametrizado, pois vou usar na aplicação esses códigos para mensagem de retorno
        THROW 51001, 'CPF Inválido.', 1;
    END;

    -- Validação do primeiro dígito validador
    -- Criação de váriaveis locais para utilização
    DECLARE @Soma INT = 0, @Resultado INT, @i INT = 1;

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
        THROW 51002, 'CPF Inválido.', 1;
    END

    -- Validação do segundo dígito, é a mesma coisa do outro porém usando os 11 dígitos agora
    SET @Soma = 0; SET @i = 1;
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
        THROW 51003, 'CPF Inválido.', 1;
    END
END