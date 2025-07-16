/**
 * Exibe feedback de validação (erro ou sucesso) no campo de um formulário específico.
 * @param {string|null} formId - ID do formulário; se for null, usa document.
 * @param {string} fieldId - ID do elemento dentro do formulário.
 * @param {string} message - Mensagem a ser exibida.
 * @param {boolean} [isValid=false] - true: sucesso; false: erro.
 * @param {boolean} [removeOnEmpty=false] - true: remove validação se o campo estiver vazio.
 */
function FeedbackElemento(formId, fieldId, message, isValid = false, removeOnEmpty = false) {
    // Obtém o contexto onde buscar o input
    let context = document;
    if (formId) {
        const form = document.getElementById(formId);
        if (!form) return;
        context = form;
    }

    // Busca o input dentro do contexto
    const input = context.querySelector(`#${fieldId}`);
    if (!input) return;

    // Remove classes e feedback anteriores
    input.classList.remove('is-invalid', 'is-valid');
    const next = input.nextElementSibling;
    if (next && (next.classList.contains('invalid-feedback') || next.classList.contains('valid-feedback'))) {
        next.remove();
    }

    // Se removeOnEmpty for true, finaliza aqui sem adicionar nada
    if (removeOnEmpty) {
        return;
    }

    // Cria e exibe novo feedback
    const feedback = document.createElement('div');
    if (isValid) {
        input.classList.add('is-valid');
        feedback.className = 'valid-feedback';
    } else {
        input.classList.add('is-invalid');
        feedback.className = 'invalid-feedback';
    }
    feedback.textContent = message;
    input.insertAdjacentElement('afterend', feedback);
}
