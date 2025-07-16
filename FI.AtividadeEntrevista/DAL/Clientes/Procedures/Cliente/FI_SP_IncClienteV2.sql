CREATE PROCEDURE [dbo].[FI_SP_IncClienteV2]
    @NOME          VARCHAR (50),
    @SOBRENOME     VARCHAR (255),
    @NACIONALIDADE VARCHAR (50),
    @CEP           VARCHAR (9),
    @ESTADO        VARCHAR (2),
    @CIDADE        VARCHAR (50),
    @LOGRADOURO    VARCHAR (500),
    @EMAIL         VARCHAR (2079),
    @TELEFONE      VARCHAR (15), 
    @CPF           VARCHAR (14)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Valida o CPF, se der erro vai estourar uma Exception
    EXEC dbo.FI_SP_VerificaCpf @CPF;

    -- Validação se tem esse CPF inserido (já tratado por unique constraint na tabela)
    EXEC dbo.FI_SP_VerificaCliente @CPF;
    IF @@ROWCOUNT > 0
    BEGIN
        THROW 51004, 'CPF inválido.', 1;
    END

    -- Será executado após validar esse CPF, fiz por procedure para pegar o erro específico
    -- Ajustando a mensagem de retorno ao usuário 
    --BEGIN TRY
    INSERT INTO CLIENTES
        (NOME, SOBRENOME, NACIONALIDADE, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, CPF)
    VALUES
        (@NOME, @SOBRENOME, @NACIONALIDADE, @CEP, @ESTADO, @CIDADE, @LOGRADOURO, @EMAIL, @TELEFONE, @CPF);
    --END TRY
    --BEGIN CATCH
    --    THROW 50001, 'Erro interno ao salvar os dados, favor revisar o preenchimento.', 1;
    --END CATCH

    -- Retorno o ID gerado
    SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS NovoId;
END