const btn_login = document.getElementById("btn_login")
const div_msg = document.getElementById("div_msg")

btn_login.addEventListener("click",async(e)=>{
    e.preventDefault()
    const email_user = document.getElementById("email_user").value
    const senha_user = document.getElementById("senha_user").value
    if(email_user == "" || senha_user == ""){
        div_msg.textContent = "Prencha todos os campos"
        div_msg.style.color = "red"
        setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
    }

    try {
        const req = await fetch("http://localhost:5280/api/Responsavel/login",{
            method:"Post",
            headers:{"Content-Type":"application/json"},
            credentials:"include",
            body:JSON.stringify({credencial:email_user,password:senha_user})
        })
        const res = await req.json()
        if(res.Success){
            localStorage.setItem("token", res.Token)
            div_msg.textContent = res.Message
            div_msg.style.color = "green"
            setTimeout(()=>{
            div_msg.textContent = ""
            },2000)
            window.location.href = "../../pages/pages_ini/Dashboard.html"
        }else{
            div_msg.textContent = res.Message
            div_msg.style.color = "red"
            setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
        }
    } catch (error) {
            div_msg.textContent = "Erro interno"
            div_msg.style.color = "red"
            setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
    }
})