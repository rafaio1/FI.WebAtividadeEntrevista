ALTER TABLE dbo.Beneficiarios
ADD CONSTRAINT FI_UK_Beneficiarios_IDCliente_CPF
    UNIQUE (IDCliente, CPF);
GO