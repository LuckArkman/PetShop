const btn_login_rec = document.getElementById("btn_login_rec")
const div_msg = document.getElementById("div_msg")
    btn_login_rec.addEventListener("click",async(e)=>{
        e.preventDefault()
        const rg_rec = document.getElementById("rg_rec").value
        const senha_rec = document.getElementById("senha_rec").value
        if(rg_rec == "" || senha_rec == ""){
            div_msg.textContent = "Prencha todos os campos"
            div_msg.style.color = "red"
            setTimeout(()=>{
                div_msg.textContent = ""
            },2000)
        }
    
        try {
            const req = await fetch("https://petrakka.com:7231/api/Atendente/login",{
                method:"Post",
                headers:{"Content-Type":"application/json"},
                credentials:"include",
                body:JSON.stringify({credencial:rg_rec,password:senha_rec})
            })
            const res = await req.json()
            console.log(res)
            if(res.success){
                localStorage.setItem("token", res.token)
                div_msg.textContent = res.message
                div_msg.style.color = "green"
                setTimeout(()=>{
                div_msg.textContent = ""
                },2000)
                window.location.href = "../../pages/pages_ini/Page_rec.html"
            }else{
                div_msg.textContent = res.message
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
