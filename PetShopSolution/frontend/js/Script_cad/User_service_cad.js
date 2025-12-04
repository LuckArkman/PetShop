const btn_cad = document.getElementById("btn_cad")
const div_msg = document.getElementById("div_msg")

    btn_cad.addEventListener("click",async (e)=>{
        e.preventDefault()
        const username_user = document.getElementById("username_user").value
        const email_user = document.getElementById("email_user").value
        const senha_user = document.getElementById("senha_user").value
        const confirm_senha_user = document.getElementById("confirm_senha_user").value
        const rg_user = document.getElementById("rg_user").value
        if(senha_user!=confirm_senha_user){
            div_msg.style.color = "red"
            div_msg.textContent = "Senhas não coincidem"
            setTimeout(()=>{
                div_msg.textContent = ""
            },2000)
            return
        }
        if(username_user==""||email_user==""||senha_user==""||confirm_senha_user=="" || rg_user==""){
            div_msg.style.color = "red"
            div_msg.textContent = "Preencha todos os campos"
            setTimeout(()=>{
                div_msg.textContent = ""
            },2000)
            return
        }
        try {
            const req = await fetch("https://petrakka.com:7231/api/Responsavel/register",{
                method:"Post",
                headers:{"Content-Type":"application/json"},
                body:JSON.stringify({Email:email_user,Password:senha_user,ConfirmPassword:confirm_senha_user,FirstName:username_user,RG:rg_user})
            })
            const res = await req.json()
            console.log(res)
            if(req.status === 401){
                div_msg.style.color = "red"
                div_msg.textContent = res.message
            }
            if(res.message){
                div_msg.style.color = "green"
                div_msg.textContent = "Cadastro realizado com sucesso"
                setTimeout(()=>{
                    div_msg.textContent = ""
                },2000)
                window.location.href = "../../pages/pages_login/Login_user.html"
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

export default cad_user