const btn_perfil = document.getElementById("btn_perfil")

btn_perfil.addEventListener("click",()=>{
    const card_perfil_config = document.getElementById("card_perfil_config")
    card_perfil_config.classList.toggle("show")
})

config_user.addEventListener("click",(e)=>{
    window.location.href = "../../pages/pages_ini/Perfil_user.html"
})

sair_user.addEventListener("click",(e)=>{
    window.location.href = "../../pages/pages_login/Login_user.html"
    window.localStorage.clear()
})