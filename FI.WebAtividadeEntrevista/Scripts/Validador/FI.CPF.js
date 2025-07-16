/**
 * Verifica se um CPF é válido.
 * @param {string} cpf - CPF com ou sem máscara (pontos e hífen).
 * @returns {boolean} Retorna true se o CPF for válido, caso contrário lança um erro com código específico.
 */
function verificaCpf(cpf) {
    // Removo mascara do CPF mesmo que não tenha para prevenção
    const cpfLimpo = String(cpf).replace(/\D+/g, '');

    // Verifica comprimento da string resultante e verifica se não está usando números repetidos tipo 111.111.111-11
    if (cpfLimpo.length !== 11 || /^([0-9])\1{10}$/.test(cpfLimpo)) {
        // Deixei os códigos de erro parametrizado, pois vou usar na aplicação esses códigos para mensagem de retorno
        const error = new Error('CPF Inválido.');
        throw error;
    }

    // Validação do primeiro dígito validador
    // Criação de váriaveis locais para utilização
    let soma = 0;
    let resultado;

    // Uso os 10 primeiros dígitos, porque os 2 últimos são os de validação
    for (let i = 0; i < 9; i++) {
        // Essa é uma função otimizada, a ideia é somar o peso de uma cadeia de caracteres
        // Exemplo:  1000000002 onde esse número '1' tem o peso 10 e o número '2' tem o peso 1
        // então seria o somatório de cada uma dessas posições, multiplicado pelo peso dela
        // Seria esse cálculo: 1×10+ 0×9+ 0×8+ 0×7+ 0×6+ 0×5+ 0×4+ 0×3+ 0×2+ 2×1 = 12
        soma += parseInt(cpfLimpo.charAt(i), 10) * (11 - (i + 1));
        // Pulo para o próximo dígito (implícito pelo loop)
    }

    // Uso o 11 e subtrario o resto da divisão por 11 desse somatório que fizemos
    resultado = 11 - (soma % 11);

    // Se esse somatório der 10, o dígito validador é 0
    if (resultado === 10) resultado = 0;

    // Agora testo se o meu dígito calculado é o mesmo do cpf na posição 11
    if (resultado !== parseInt(cpfLimpo.charAt(9), 10)) {
        const error = new Error('CPF Inválido.');
        throw error;
    }

    // Validação do segundo dígito, é a mesma coisa do outro porém usando os 11 dígitos agora
    soma = 0;
    for (let i = 0; i < 10; i++) {
        soma += parseInt(cpfLimpo.charAt(i), 10) * (12 - (i + 1));
    }
    resultado = 11 - (soma % 11);

    // Se esse somatório der 10, o dígito validador é 0
    if (resultado === 10) resultado = 0;

    // Agora testo se o meu dígito calculado é o mesmo do cpf na posição 12
    if (resultado !== parseInt(cpfLimpo.charAt(10), 10)) {
        const error = new Error('CPF Inválido.');
        throw error;
    }

    return true;
}

/**
 * Formata um CPF limpo (somente dígitos) com máscara de pontos e hífen.
 * @param {string} cpf - CPF com ou sem máscara.
 * @returns {string} CPF formatado no padrão XXX.XXX.XXX-XX
 */
function formataCpf(cpf) {
    const cpfLimpo = String(cpf).replace(/\D+/g, '');
    return cpfLimpo.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
}

