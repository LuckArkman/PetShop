const btn_login = document.getElementById("btn_login")
const div_msg = document.getElementById("div_msg")
    btn_login.addEventListener("click",async(e)=>{
    e.preventDefault()
    const crmv_vet = document.getElementById("crmv_vet").value
    const senha_vet = document.getElementById("senha_vet").value
    if(senha_vet == "" || senha_vet == ""){
        div_msg.textContent = "Prencha todos os campos"
        div_msg.style.color = "red"
        setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
    }

    try {
        const req = await fetch("https://petrakka.com:7231/api/MedicoVeterinario/login",{
            method:"Post",
            headers:{"Content-Type":"application/json"},
            credentials:"include",
            body:JSON.stringify({credencial:crmv_vet,password:senha_vet})
        })
        const res = await req.json()
        if(res.success){
            div_msg.textContent = res.Message
            div_msg.style.color = "green"
            setTimeout(()=>{
            div_msg.textContent = ""
            },2000)
            window.location.href = "../../pages/pages_ini/Page_vet.html"
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
