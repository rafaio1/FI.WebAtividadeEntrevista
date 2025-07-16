
    -- Trigger está inativa, não estava no escopo como permitido ou não

CREATE TRIGGER dbo.FI_TR_IncCliente_OnInsert
ON dbo.CLIENTES
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE 
        @NOME          VARCHAR(50),
        @SOBRENOME     VARCHAR(255),
        @NACIONALIDADE VARCHAR(50),
        @CEP           VARCHAR(9),
        @ESTADO        VARCHAR(2),
        @CIDADE        VARCHAR(50),
        @LOGRADOURO    VARCHAR(500),
        @EMAIL         VARCHAR(2079),
        @TELEFONE      VARCHAR(15),
        @CPF           VARCHAR(14);

    DECLARE cliente_cursor CURSOR LOCAL FAST_FORWARD
    FOR
        SELECT NOME, SOBRENOME, NACIONALIDADE, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, CPF
        FROM inserted;

    OPEN cliente_cursor;
    FETCH NEXT FROM cliente_cursor
      INTO @NOME, @SOBRENOME, @NACIONALIDADE, @CEP, @ESTADO, @CIDADE, @LOGRADOURO, @EMAIL, @TELEFONE, @CPF;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.FI_SP_VerificaCpf @CPF;

        EXEC dbo.FI_SP_VerificaCliente @CPF;
        IF @@ROWCOUNT > 0
        BEGIN
            CLOSE cliente_cursor;
            DEALLOCATE cliente_cursor;
            THROW 51004, 'CPF inválido.', 1;
        END

        BEGIN TRY
            INSERT INTO dbo.CLIENTES
                (NOME, SOBRENOME, NACIONALIDADE, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, CPF)
            VALUES
                (@NOME, @SOBRENOME, @NACIONALIDADE, @CEP, @ESTADO, @CIDADE, @LOGRADOURO, @EMAIL, @TELEFONE, @CPF);
        END TRY
        BEGIN CATCH
            CLOSE cliente_cursor;
            DEALLOCATE cliente_cursor;
            THROW 50001, 'Erro interno ao salvar os dados, favor revisar o preenchimento.', 1;
        END CATCH;

        FETCH NEXT FROM cliente_cursor
          INTO @NOME, @SOBRENOME, @NACIONALIDADE, @CEP, @ESTADO, @CIDADE, @LOGRADOURO, @EMAIL, @TELEFONE, @CPF;
    END

    CLOSE cliente_cursor;
    DEALLOCATE cliente_cursor;
END;
GO
