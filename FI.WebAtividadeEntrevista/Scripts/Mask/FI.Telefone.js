(function (window) {
    /**
     * Máscara para telefone brasileiro: (##) ####-#### ou (##) #####-####
     */
    class TelefoneMask {
        /**
         * @param {string} selector Seletor CSS (por padrão, inputs com data-mask="telefone")
         */
        constructor(selector = 'input[data-mask="telefone"]') {
            this.selector = selector;
            this._onInput = this._onInput.bind(this);
        }

        /**
         * Formata o valor:
         * - até 2 dígitos: "(##"
         * - entre 3 e 6 dígitos: "(##) ####"
         * - entre 7 e 10 dígitos: "(##) ####-####"
         * - 11 dígitos: "(##) #####-####"
         * @param {string} value
         * @returns {string}
         */
        _format(value) {
            const digits = value.replace(/\D/g, '').slice(0, 11);

            if (digits.length > 10) {
                // celular: 11 dígitos
                return digits.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
            } else if (digits.length > 6) {
                // telefone fixo: 10 dígitos
                return digits.replace(/(\d{2})(\d{4})(\d+)/, '($1) $2-$3');
            } else if (digits.length > 2) {
                // adiciona DDD
                return digits.replace(/(\d{2})(\d+)/, '($1) $2');
            } else if (digits.length > 0) {
                // começando o DDD
                return digits.replace(/(\d{1,2})/, '($1');
            }
            return '';
        }

        _onInput(e) {
            const el = e.target;
            const formId = el.form ? el.form.id : null;

            // guarda posição antiga em termos de dígitos
            const oldPos = el.selectionStart;
            const rawBefore = el.value.slice(0, oldPos).replace(/\D/g, '').length;

            // aplica máscara
            el.value = this._format(el.value);

            // reposiciona cursor corretamente
            let digitCount = 0;
            let newPos = el.value.length;
            for (let i = 0; i < el.value.length; i++) {
                if (/\d/.test(el.value[i])) digitCount++;
                if (digitCount === rawBefore) {
                    newPos = i + 1;
                    break;
                }
            }
            el.setSelectionRange(newPos, newPos);

            // Valida quando tiver 11 dígitos
            const raw = el.value.replace(/\D/g, '');
            if (raw.length === 11) {
                FeedbackElemento(formId, el.id, 'Telefone válido.', true);
            }
            else if (raw.length > 0 && raw.length < 10) {
                FeedbackElemento(formId, el.id, 'Telefone inválido.', false);
            }
            else {
                FeedbackElemento(formId, el.id, '', null, true);
            }
        }

        init() {
            this.inputs = Array.from(document.querySelectorAll(this.selector));
            this.inputs.forEach(input => {
                input.setAttribute('maxlength', '15');
                const formId = input.form ? input.form.id : null;

                if (input.value) {
                    input.value = this._format(input.value);
                    const raw = input.value.replace(/\D/g, '');

                    if (raw.length === 11) {
                        FeedbackElemento(formId, input.id, 'Telefone válido.', true);
                    }
                    else if (raw.length > 0 && raw.length < 10) {
                        FeedbackElemento(formId, input.id, 'Telefone inválido.', false);
                    }
                    else {
                        FeedbackElemento(formId, input.id, '', null, true);
                    }
                }

                ['input', 'paste', 'cut', 'reset'].forEach(evt => {
                    input.addEventListener(evt, e => this._onInput(e));
                });
            });
        }

        destroy() {
            this.inputs.forEach(input => input.removeEventListener('input', this._onInput));
            this.inputs = [];
        }
    }

    window.TelefoneMask = TelefoneMask;
})(window);
