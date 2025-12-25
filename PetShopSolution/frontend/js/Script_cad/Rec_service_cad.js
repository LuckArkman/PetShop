const btn_cad = document.getElementById("btn_cad_rec")
const div_msg = document.getElementById("div_msg")

btn_cad.addEventListener("click",async (e)=>{
    e.preventDefault()
    const username_rec = document.getElementById("username_rec").value
    const email_rec = document.getElementById("email_rec").value
    const senha_rec = document.getElementById("senha_rec").value
    const confirm_senha_rec = document.getElementById("confirm_senha_rec").value
    const rg_rec = document.getElementById("rg_rec").value
    if(senha_rec!=confirm_senha_rec){
        div_msg.style.color = "red"
        div_msg.textContent = "Senhas não coincidem"
        setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
        return
    }
    if(username_rec==""||email_rec==""||senha_rec==""||confirm_senha_rec=="" || rg_rec==""){
        div_msg.style.color = "red"
        div_msg.textContent = "Preencha todos os campos"
        setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
        return
    }
    try {
        const req = await fetch("https://petrakka.com:7231/api/Atendente/register",{
            method:"Post",
            headers:{"Content-Type":"application/json"},
            body:JSON.stringify({email:email_rec,password:senha_rec,confirmPassword:confirm_senha_rec,nome:username_rec,rg:rg_rec})
        })
        const res = await req.json()

        if(res.message){
            div_msg.style.color = "green"
            div_msg.textContent = "Cadastro realizado com sucesso"
            setTimeout(()=>{
                div_msg.textContent = ""
            },2000)
            window.location.href = "../../pages/pages_login/Login_rec.html"
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
