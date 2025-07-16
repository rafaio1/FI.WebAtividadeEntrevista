CREATE PROCEDURE dbo.FI_SP_AltCliente
    @NOME          VARCHAR (50),
    @SOBRENOME     VARCHAR (255),
    @NACIONALIDADE VARCHAR (50),
    @CEP           VARCHAR (9),
    @ESTADO        VARCHAR (2),
    @CIDADE        VARCHAR (50),
    @LOGRADOURO    VARCHAR (500),
    @EMAIL         VARCHAR (2079),
    @TELEFONE      VARCHAR (15),
    @CPF           VARCHAR (14),
    @Id            BIGINT
AS
BEGIN
    SET NOCOUNT, XACT_ABORT ON;
    
    -- Valida o CPF, se der erro vai estourar uma Exception
    EXEC dbo.FI_SP_VerificaCpf @CPF;
    
    -- Validação se tem esse CPF inserido (já tratado por unique constraint na tabela)
    EXEC dbo.FI_SP_VerificaCliente
    @CPF       = @CPF,
    @IDCliente = @Id;
    IF @@ROWCOUNT > 0
    BEGIN
        THROW 51004, 'CPF inválido.', 1;
    END
    
    -- Será executado após validar esse CPF, fiz por procedure para pegar o erro específico
    -- Ajustando a mensagem de retorno ao usuário 
    BEGIN TRY
        UPDATE CLIENTES
        SET
            NOME          = @NOME,
            SOBRENOME     = @SOBRENOME,
            NACIONALIDADE = @NACIONALIDADE,
            CEP           = @CEP,
            ESTADO        = @ESTADO,
            CIDADE        = @CIDADE,
            LOGRADOURO    = @LOGRADOURO,
            EMAIL         = @EMAIL,
            TELEFONE      = @TELEFONE,
            CPF           = @CPF
        WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        THROW 50002, 'Erro interno ao atualizar os dados, favor revisar o preenchimento.', 1;
    END CATCH

    SELECT @Id AS IdAlterado;
END
