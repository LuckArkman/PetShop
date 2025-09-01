const btn_perfil = document.getElementById("btn_perfil")

btn_perfil.addEventListener("click",()=>{
    const card_perfil_config = document.getElementById("card_perfil_config")
    card_perfil_config.classList.toggle("show")
})