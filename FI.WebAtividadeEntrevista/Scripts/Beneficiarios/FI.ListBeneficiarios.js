
// Solicitado para não alterar o frontend
//$(document).ready(function () {

//    if (document.getElementById("gridBeneficiario"))
//    {
//        $('#gridBeneficiario').jtable({
//            title: 'Beneficiários',
//            paging: true, //Enable paging
//            pageSize: 5, //Set page size (default: 10)
//            sorting: true, //Enable sorting
//            defaultSorting: 'Nome ASC', //Set default sorting
//            actions: {
//                listAction: urlBeneficiarioList,
//            },
//            fields: {
//                CPF: {
//                    title: 'CPF',
//                    width: '35%'
//                },
//                Nome: {
//                    title: 'Nome',
//                    width: '35%'
//                },
//                Alterar: {
//                    title: '',
//                    display: function (data) {
//                        return `<button onclick="AlterarBeneficiario(${data.record.CPF})" class="btn btn-info btn-sm">Alterar</button>`;
//                        return `<button onclick="ExcluirBeneficiario(${data.record.CPF})" class="btn btn-info btn-sm">Alterar</button>`;
//                    }
//                }
//            }
//        });

//    }

//    //Load student list from server
//    if (document.getElementById("gridClientes"))
//        $('#gridClientes').jtable('load');
//})