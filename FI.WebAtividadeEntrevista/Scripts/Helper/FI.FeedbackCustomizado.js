/**
 * Exibe feedback de validação (erro ou sucesso) no campo.
 * @param {string} fieldId - ID do elemento.
 * @param {string} message - Mensagem a ser exibida.
 * @param {boolean} [isValid=false] - true: sucesso; false: erro.
 * @param {boolean} [removeOnEmpty=false] - true: remove validação se o campo estiver vazio.
 */
function FeedbackElemento(fieldId, message, isValid = false, remover = false) {
    const input = document.getElementById(fieldId);
    if (!input) return;

    // Remove classes e feedback anteriores
    input.classList.remove('is-invalid', 'is-valid');
    const next = input.nextElementSibling;
    if (next && (next.classList.contains('invalid-feedback') || next.classList.contains('valid-feedback'))
    ) {
        next.remove();
    }

    // Se ativado e o campo estiver vazio, encerra sem adicionar nova validação
    if (remover) {
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
