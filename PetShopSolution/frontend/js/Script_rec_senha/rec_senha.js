const cpf_user = document.getElementById("cpf_user")
const btn_reset_senha = document.getElementById("btn_reset_senha")

cpf_user.addEventListener("input", () => {
    let cpf = cpf_user.value.replace(/\D/g, "")
    if (cpf.length > 11) cpf = cpf.slice(0, 11)
    cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2")
    cpf = cpf.replace(/(\d{3})(\d)/, "$1.$2")
    cpf = cpf.replace(/(\d{3})(\d{1,2})$/, "$1-$2")
    cpf_user.value = cpf
})

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

btn_reset_senha.addEventListener("click",async(e)=>{
    const email_user = document.getElementById("email_user").value
    const senha_user = document.getElementById("senha_user").value
    const confirm_senha_user = document.getElementById("confirm_senha_user").value
    const dados = {
        email:email_user,
        cpf:cpf_user.value.replace(/[^\dXx]/g, ""),
        password:senha_user,
        confirmPassword:confirm_senha_user
    }
    console.log(dados)
    if(senha_user !==confirm_senha_user){
        alert("Senhas não coincidem!")
        return
    }
    
    if (!validarCPF(cpf_user.value)) {
        alert("CPF inválido")
        return
    }
    console.log(dados)
    try {
        const req_att_senha = await fetch("https://petrakka.com:7231/api/Responsavel/update",{
            method:"PUT",
            headers:{"Content-Type":"application/json"},
            body:JSON.stringify(dados)
        })
        const res_att_senha = await req_att_senha.json()
        console.log(res_att_senha)
        if(res_att_senha.user){
            alert("Senha atualizado com sucesso")
            window.location.href = "../pages_login/Login_user.html"
        }
    } catch (error) {
        console.log(error)
    }
})