(function (window) {
    /**
     * @callback OnInvalidCallback
     * @param {Error} error
     */
    class CPFMask {
        /**
         * @param {string} selector O seletor de CSS
         * @param {OnInvalidCallback} onInvalid A função que vai ser executada caso dê erro
         */
        constructor(selector = 'input[data-mask="cpf"]', onInvalid = null) {
            this.selector = selector;
            this.onInvalid = onInvalid;
            this._onInput = this._onInput.bind(this);
        }

        _format(value) {
            const digits = value.replace(/\D/g, '').slice(0, 11);
            let m = digits;
            if (digits.length > 3) m = digits.replace(/(\d{3})(\d+)/, '$1.$2');
            if (digits.length > 6) m = m.replace(/(\d{3})\.(\d{3})(\d+)/, '$1.$2.$3');
            if (digits.length > 9) m = m.replace(/(\d{3})\.(\d{3})\.(\d{3})(\d{1,2})/, '$1.$2.$3-$4');
            return m;
        }

        _onInput(e) {
            const el = e.target;

            // Quantidade de dígitos no cursor de digitação (evitar pular posição ou voltar)
            const oldPos = el.selectionStart;
            const rawBefore = el.value.slice(0, oldPos).replace(/\D/g, '').length;

            // Formata o valor
            el.value = this._format(el.value);

            // Reposiciona o cursor de digitação do usuário
            let digitCount = 0;
            let newPos = el.value.length;
            for (let i = 0; i < el.value.length; i++) {
                if (/\d/.test(el.value[i])) digitCount++;
                if (digitCount === rawBefore) { newPos = i + 1; break; }
            }
            el.setSelectionRange(newPos, newPos);

            // Se for 11 dígitos, válida o cpf
            const raw = el.value.replace(/\D/g, '');
            if (raw.length === 11) {
                try {
                    try {
                        verificaCpf(raw);

                        FeedbackElemento(el.id, 'CPF válido.', true);
                        if (typeof this.onValid === 'function') this.onValid(raw);

                    }
                    catch (err) {
                        FeedbackElemento(el.id, err.message, false);
                        if (typeof this.onInvalid === 'function') this.onInvalid(err);
                    }
                } catch (err) {
                    if (typeof this.onInvalid === 'function') {
                        this.onInvalid(err);
                    }
                }
            }
            else {
                FeedbackElemento(el.id, '', null, true);
            }
        }

        init() {
            this.inputs = Array.from(document.querySelectorAll(this.selector));
            this.inputs.forEach(input => {
                input.setAttribute('maxlength', '14');
                if (input.value) {
                    input.value = this._format(input.value);
                    const raw = el.value.replace(/\D/g, '');
                    if (raw.length === 11) {
                        try {
                            try {
                                verificaCpf(raw);

                                FeedbackElemento(el.id, 'CPF válido.', true);
                                if (typeof this.onValid === 'function') this.onValid(raw);

                            }
                            catch (err) {
                                FeedbackElemento(el.id, err.message, false);
                                if (typeof this.onInvalid === 'function') this.onInvalid(err);
                            }
                        } catch (err) {
                            if (typeof this.onInvalid === 'function') {
                                this.onInvalid(err);
                            }
                        }
                    }
                    else {
                        FeedbackElemento(el.id, '', null, true);
                    }
                }

                input.addEventListener('input', this._onInput);
            });
        }

        destroy() {
            this.inputs.forEach(input => input.removeEventListener('input', this._onInput));
            this.inputs = [];
        }
    }

    window.CPFMask = CPFMask;
})(window);
