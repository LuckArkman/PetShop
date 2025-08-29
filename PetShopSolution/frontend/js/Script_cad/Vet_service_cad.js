const btn_cad = document.getElementById("btn_cad")
const div_msg = document.getElementById("div_msg")

btn_cad.addEventListener("click",async (e)=>{
    e.preventDefault()
    const nome_user = document.getElementById("nome_user").value
    const email_user = document.getElementById("email_user").value
    const crmv_user = document.getElementById("crmv_user").value
    const especialidade_user = document.getElementById("especialidade_user").value
    const telefone_user = document.getElementById("telefone_user").value
    const senha_user = document.getElementById("senha_user").value
    const confirm_senha_user = document.getElementById("confirm_senha_user").value
    if(senha_user!=confirm_senha_user){
        div_msg.style.color = "red"
        div_msg.textContent = "Senhas não coincidem"
        setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
        return
    }
    if(nome_user==""||email_user==""||senha_user==""||confirm_senha_user==""||crmv_user ==""||especialidade_user==""||telefone_user==""){
        div_msg.style.color = "red"
        div_msg.textContent = "Preencha todos os campos"
        setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
        return
    }
    try {
        const req = await fetch("http://localhost:5280/api/MedicoVeterinario/register",{
            method:"Post",
            headers:{"Content-Type":"application/json"},
            body:JSON.stringify({Nome:nome_user,CRMV:crmv_user,Password:senha_user,ConfirmPassword:confirm_senha_user,Especialidade:especialidade_user,Telefone:telefone_user,Email:email_user})
        })
        const res = await req.json()
        if(res && res.Id){
            div_msg.style.color = "green"
            div_msg.textContent = "Cadastro realizado com sucesso"
            setTimeout(()=>{
                div_msg.textContent = "" 
            },2000)
            window.location.href = "../../pages/pages_login/Login_vet.html"
        }else{
            div_msg.style.color = "red"
            div_msg.textContent = "Erro ao se cadastrar"
            setTimeout(()=>{
                div_msg.textContent = ""
            },2000)
            return
        }
        
        
    } catch (error) {
        div_msg.style.color = "red"
        div_msg.textContent = "Erro interno"
        setTimeout(()=>{
            div_msg.textContent = "" 
        },2000)
        return
    }
})