CREATE PROCEDURE [dbo].[FI_SP_AltBeneficiario]
    @NOME          VARCHAR (50),
    @CPF           VARCHAR (14),
    @Id            BIGINT,
    @IdCliente     BIGINT
AS
BEGIN
    SET NOCOUNT, XACT_ABORT ON;
    
    -- Ajustando a mensagem de retorno ao usuário 
    BEGIN TRY
        UPDATE BENEFICIARIOS
        SET
            NOME          = @NOME,
            CPF           = @CPF,
            IDCLIENTE     = @IdCliente
        WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        THROW 50002, 'Erro interno ao atualizar os dados, favor revisar o preenchimento.', 1;
    END CATCH

    SELECT @Id;
END
