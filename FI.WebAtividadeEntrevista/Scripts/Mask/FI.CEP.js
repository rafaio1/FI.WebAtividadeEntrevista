(function (window) {
    /**
     * @callback OnInvalidCallback
     * @param {Error} error
     */
    class CEPMask {
        /**
         * @param {string} selector O seletor de CSS
         * @param {OnInvalidCallback} onInvalid A função que vai ser executada caso dê erro
         */
        constructor(selector = 'input[data-mask="cep"]', onInvalid = null) {
            this.selector = selector;
            this.onInvalid = onInvalid;
            this._onInput = this._onInput.bind(this);
        }

        /**
         * Formata o valor para #####-###
         * @param {string} value
         * @returns {string}
         */
        _format(value) {
            const digits = value.replace(/\D/g, '').slice(0, 8);
            let m = digits;
            if (digits.length > 5) {
                m = digits.replace(/(\d{5})(\d+)/, '$1-$2');
            }
            return m;
        }

        _onInput(e) {
            const el = e.target;
            const formId = el.form ? el.form.id : null;

            // Preserva posição do cursor em termos de dígitos
            const oldPos = el.selectionStart;
            const rawBefore = el.value.slice(0, oldPos).replace(/\D/g, '').length;

            // Aplica máscara
            el.value = this._format(el.value);

            // Reposiciona cursor
            let digitCount = 0;
            let newPos = el.value.length;
            for (let i = 0; i < el.value.length; i++) {
                if (/\d/.test(el.value[i])) digitCount++;
                if (digitCount === rawBefore) { newPos = i + 1; break; }
            }
            el.setSelectionRange(newPos, newPos);

            // Valida quando tiver 8 dígitos
            const raw = el.value.replace(/\D/g, '');
            if (raw.length === 8) {
                FeedbackElemento(formId, el.id, 'CEP válido.', true);
            }
            else if (raw.length > 0) {
                FeedbackElemento(formId, el.id, 'CEP inválido.', false);
            }
            else {
                // Quando não enviar se é válido ou não e adicionar o true ao final irá remover o feedback
                FeedbackElemento(formId, el.id, '', null, true);
            }
        }

        init() {
            this.inputs = Array.from(document.querySelectorAll(this.selector));
            this.inputs.forEach(input => {
                input.setAttribute('maxlength', '9');

                const formId = input.form ? input.form.id : null;
                if (input.value) {
                    input.value = this._format(input.value);
                    const raw = input.value.replace(/\D/g, '');
                    if (raw.length === 8) {
                        FeedbackElemento(formId, input.id, 'CEP válido.', true);
                    } else if (raw.length > 0) {
                        FeedbackElemento(formId, input.id, 'CEP inválido.', false);
                    } else {
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

    window.CEPMask = CEPMask;
})(window);
