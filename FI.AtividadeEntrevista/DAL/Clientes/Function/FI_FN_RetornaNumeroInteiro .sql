-- =============================================
-- Author:      Rafael Antunes
-- Create date: 15/07/2025 
-- Description: Função que retorna somente os números de um varchar
-- =============================================
CREATE FUNCTION dbo.FI_FN_RetornaNumeroInteiro    
(
    @Texto VARCHAR(50)  
)
RETURNS BIGINT
AS
BEGIN
    -- Declaração de variáveis
    DECLARE @Resultado VARCHAR(50) = '';
    DECLARE @Indice INT = 1;
    DECLARE @Comprimento INT = LEN(@Texto);

    -- Loop para percorrer cada caractere
    WHILE @Indice <= @Comprimento
    BEGIN
        -- Se o caractere for dígito, concatena no resultado
        IF SUBSTRING(@Texto, @Indice, 1) LIKE '[0-9]'
            SET @Resultado += SUBSTRING(@Texto, @Indice, 1);

        SET @Indice += 1;
    END

    -- Converte para inteiro, retornando 0 se não houver dígitos
    RETURN CASE WHEN @Resultado = '' THEN 0 ELSE CAST(@Resultado AS BIGINT) END;
END;
