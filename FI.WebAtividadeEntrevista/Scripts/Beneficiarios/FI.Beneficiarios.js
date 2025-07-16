
$(function () {
    //Seletores
    const $form = $('#formCadastroBeneficiario');
    const $btnSalvar = $('#btnSalvarBeneficiario');
    const $btnAlterar = $('#btnAlterarBeneficiario');
    const $tableBody = $('#tabelaBeneficiarios tbody');
    const $modalDelete = $('#confirmDeleteModal');
    const $confirmBtn = $('#confirmDeleteBtn');
    const $nameSpan = $('#confirmDeleteName');
    const $cpfInput = $form.find('input[name="CPF"]');

    let $selectedRow = null;

    function carregarBeneficiarios() {
        $.post('/Beneficiario/BeneficiarioList', {
            jtStartIndex: 0,
            jtPageSize: 1000,
            jtSorting: 'Nome ASC',
            guid: guid
        })
            .done(data => {
                if (data.Result === 'OK') {
                    $tableBody.empty();
                    data.Records.forEach(item => {
                        $tableBody.append(construirLinha(item));
                    });
                } else {
                    ModalDialogBeneficiario("Erro ao carregar beneficiários.", data.Message, idModalBeneficiario)
                }
            })
            .fail(response => {
                ModalDialogBeneficiario("Erro ao carregar beneficiários.", data.Message, idModalBeneficiario)
            });
    }

    $cpfInput.on('input paste', function () {
        const rawCpf = $(this).val().replace(/\D/g, '');

        // só prossegue quando tiver 11 dígitos
        if (rawCpf.length !== 11) return;

        const $foundRow = $tableBody.find(`tr[data-cpf="${$(this).val()}"]`);

        if ($foundRow.length) {
            // alerta o usuário
            ModalDialogBeneficiario(
                "Beneficiário Já Cadastrado",
                "Já existe um cadastro com esse CPF. Carregando para alteração…",
                idModalBeneficiario
            );

            // preenche o form com os dados da linha encontrada
            fillForm($foundRow);
        }
    });

    // Adiciona a tabela
    function construirLinha(item) {
        return $(`
            <tr data-id="${item.Id}"
                data-idcliente="${item.IDCliente}"
                data-guid="${item.Guid || $form.find('#Guid').val()}"
                data-cpf="${item.CPF || $form.find('#CPF').val()}"
                data-nome="${item.Nome || $form.find('#Nome').val()}">
                <td>${item.CPF}</td>
                <td>${item.Nome}</td>
                <td>
                  <button type="button" class="btn btn-sm btn-primary btn-edit">Alterar</button>
                  <button type="button" class="btn btn-sm btn-primary btn-delete">Excluir</button>
                </td>
            </tr>
        `);
    }

    // Limpa e configura o form para inclusão
    function resetForm() {
        $form[0].reset();
        $form.find('#Id').val('');
        $btnAlterar.hide();
        $btnSalvar.show();
    }

    // Preenche o form com os dados da linha para edição
    function fillForm($tr) {
        $form.find('#Id').val($tr.data('id'));
        $form.find('#Guid').val($tr.data('guid'));
        $form.find('#IdCliente').val($tr.data('idcliente'));
        $form.find('#CPF').val($tr.data('cpf'));
        $form.find('#Nome').val($tr.data('nome'));
        $btnSalvar.hide();
        $btnAlterar.show();
        $('html, body').animate({ scrollTop: $form.offset().top }, 400);
    }

    // Carrega ao inicializar a página
    carregarBeneficiarios();

    // Incluir
    $btnSalvar.on('click', e => {
        e.preventDefault();
        const payload = {
            Nome: $form.find('#Nome').val(),
            CPF: $form.find('#CPF').val(),
            IDCliente: $form.find('#IdCliente').val(),
            guid: $form.find('#Guid').val()
        };

        $.post('/Beneficiario/Incluir', payload)
            .done(msg => {
                ModalDialogBeneficiario("Beneficiário Incluído", msg, idModalBeneficiario)
                carregarBeneficiarios();
                resetForm();
            })
            .fail(response => {
                ModalDialogBeneficiario("Erro ao incluír.Tente novamente.", response, idModalBeneficiario)
            });
    });

    // Alterar
    $btnAlterar.on('click', e => {
        e.preventDefault();
        const payload = {
            Id: $form.find('#Id').val(),
            Nome: $form.find('#Nome').val(),
            CPF: $form.find('#CPF').val(),
            IDCliente: $form.find('#IdCliente').val(),
            guid: $form.find('#Guid').val()
        };
        $.post('/Beneficiario/Alterar', payload)
            .done(msg => {
                ModalDialogBeneficiario("Beneficiário Alterado", msg, idModalBeneficiario)
                carregarBeneficiarios();
                resetForm();
            })
            .fail(response => {
                ModalDialogBeneficiario("Erro ao alterar.Tente novamente.", response, idModalBeneficiario)
            });
    });

    // Trigger do botão de edição
    $tableBody.on('click', '.btn-edit', function () {
        fillForm($(this).closest('tr'));
    });

    // Trigger do botão de exclusão
    $tableBody.on('click', '.btn-delete', function () {
        $selectedRow = $(this).closest('tr');
        const id = $selectedRow.data('id');
        const guid = $selectedRow.data('guid');
        const nome = $selectedRow.find('td').eq(1).text().trim();

        // Preenche o nome no modal e guarda id+guid no botão de confirmar
        $nameSpan.text(nome);
        $confirmBtn
            .data('id', id)
            .data('guid', guid);

        $('#' + idModalBeneficiario).modal('hide');
        $modalDelete.modal('show');
    });

    // Quando confirmar dispara a trigger
    $confirmBtn.on('click', function () {
        const payload = {
            Id: $selectedRow.data('id'),
            Nome: $selectedRow.data('nome'),
            CPF: $selectedRow.data('cpf'),
            IDCliente: $selectedRow.data('idcliente'),
            guid: $selectedRow.data('guid'),
        };

        $.post('/Beneficiario/Excluir', payload)
            .done(msg => {
                ModalDialogBeneficiario("Beneficiário Excluído", msg, idModalBeneficiario)
                carregarBeneficiarios();
                resetForm();
                $modalDelete.modal('hide');
                $selectedRow = null;
            })
            .fail(response => {
                ModalDialogBeneficiario("Erro ao excluir.Tente novamente.", response, idModalBeneficiario)
            });
    });
     
    // Reseta ao fechar
    $('#btnCancelarBeneficiario').on('click', resetForm);
});

function ModalDialogBeneficiario(titulo, texto, modalAnteriorId = null) {
    var random = Math.random().toString().replace('.', '');

    var htmlModal = `
    <div id="${random}" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">${titulo}</h4>
                </div>
                <div class="modal-body">
                    <p>${texto}</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                </div>
            </div>
        </div>
    </div>`;

    $('body').append(htmlModal);

    var $modalAtual = $('#' + random);

    $modalAtual.on('hidden.bs.modal', function () {
        $modalAtual.remove();

        $('#' + idModalBeneficiario).modal('show');
    });

    if (modalAnteriorId) {
        $('#' + idModalBeneficiario).modal('hide');
    }

    $modalAtual.modal('show');
}