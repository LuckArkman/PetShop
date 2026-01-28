const btn_cad = document.getElementById("btn_cad")
const div_msg = document.getElementById("div_msg")
const rg_input = document.getElementById("rg_user")
const cpf_input = document.getElementById("cpf_user")

rg_input.addEventListener("input", () => {
    let rg = rg_input.value.replace(/[^\dXx]/g, "")
    if (rg.length > 9) rg = rg.slice(0, 9)
    if (rg.length > 2 && rg.length <= 5) {
        rg = rg.replace(/^(\d{2})(\d+)/, "$1.$2")
    } else if (rg.length > 5 && rg.length <= 8) {
        rg = rg.replace(/^(\d{2})(\d{3})(\d+)/, "$1.$2.$3")
    } else if (rg.length > 8) {
        rg = rg.replace(/^(\d{2})(\d{3})(\d{3})(\w)/, "$1.$2.$3-$4")
    }
    rg_input.value = rg
})

cpf_input.addEventListener("input", () => {
    let cpf = cpf_input.value.replace(/\D/g, "")
    if (cpf.length > 11) cpf = cpf.slice(0, 11)
    cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2")
    cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2")
    cpf = cpf.replace(/(\d{3})(\d{1,2})$/, "$1-$2")
    cpf_input.value = cpf
})

function validarRG(rg) {
    rg = rg.replace(/[^\dXx]/g, "")
    return rg.length >= 7 && rg.length <= 9
}

function validarCPF(cpf) {
    cpf = cpf.replace(/\D/g, "")
    if (cpf.length !== 11) return false
    if (/^(\d)\1+$/.test(cpf)) return false

    let soma = 0
    let resto

    for (let i = 1; i <= 9; i++) {
        soma += parseInt(cpf.substring(i - 1, i)) * (11 - i)
    }

    resto = (soma * 10) % 11
    if (resto === 10 || resto === 11) resto = 0
    if (resto !== parseInt(cpf.substring(9, 10))) return false

    soma = 0
    for (let i = 1; i <= 10; i++) {
        soma += parseInt(cpf.substring(i - 1, i)) * (12 - i)
    }

    resto = (soma * 10) % 11
    if (resto === 10 || resto === 11) resto = 0
    if (resto !== parseInt(cpf.substring(10, 11))) return false

    return true
}

btn_cad.addEventListener("click", async e => {
    e.preventDefault()

    const username_user = document.getElementById("username_user").value
    const email_user = document.getElementById("email_user").value
    const senha_user = document.getElementById("senha_user").value
    const confirm_senha_user = document.getElementById("confirm_senha_user").value
    const rg_user = rg_input.value.replace(/[^\dXx]/g, "")
    const cpf_user = cpf_input.value.replace(/\D/g, "")

    if (
        username_user === "" ||
        email_user === "" ||
        senha_user === "" ||
        confirm_senha_user === "" ||
        rg_user === "" ||
        cpf_user === ""
    ) {
        div_msg.style.color = "red"
        div_msg.textContent = "Preencha todos os campos"
        setTimeout(() => div_msg.textContent = "", 2000)
        return
    }

    if (senha_user !== confirm_senha_user) {
        div_msg.style.color = "red"
        div_msg.textContent = "Senhas não coincidem"
        setTimeout(() => div_msg.textContent = "", 2000)
        return
    }

    if (!validarRG(rg_user)) {
        div_msg.style.color = "red"
        div_msg.textContent = "RG inválido"
        setTimeout(() => div_msg.textContent = "", 2000)
        return
    }

    if (!validarCPF(cpf_user)) {
        div_msg.style.color = "red"
        div_msg.textContent = "CPF inválido"
        setTimeout(() => div_msg.textContent = "", 2000)
        return
    }

    try {
        const req = await fetch("https://petrakka.com:7231/api/Responsavel/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                Email: email_user,
                Password: senha_user,
                ConfirmPassword: confirm_senha_user,
                FirstName: username_user,
                RG: rg_user,
                CPF: cpf_user
            })
        })

        const res = await req.json()

        if (req.status === 401) {
            div_msg.style.color = "red"
            div_msg.textContent = res.message
            return
        }

        if (res.message) {
            div_msg.style.color = "green"
            div_msg.textContent = "Cadastro realizado com sucesso"
            setTimeout(() => {
                div_msg.textContent = ""
                window.location.href = "../../pages/pages_login/Login_user.html"
            }, 2000)
        } else {
            div_msg.style.color = "red"
            div_msg.textContent = "Erro ao se cadastrar"
            setTimeout(() => div_msg.textContent = "", 2000)
        }
    } catch {
        div_msg.style.color = "red"
        div_msg.textContent = "Erro interno"
        setTimeout(() => div_msg.textContent = "", 2000)
    }
})

