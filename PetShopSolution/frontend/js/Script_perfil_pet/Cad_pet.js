const btn_add_pet = document.getElementById("btn_add_pet")
const backdropSimple = document.getElementById('backdropSimple')
const cancelSimple = document.getElementById("cancelSimple")
const saveSimple = document.getElementById('saveSimple')

btn_add_pet.addEventListener('click',()=>backdropSimple.style.display='flex')
cancelSimple.addEventListener('click',()=>backdropSimple.style.display='none')
document.querySelectorAll('.modal-backdrop').forEach(b=>b.addEventListener('click',e=>{if(e.target===b)b.style.display='none'}))

    function getPayloadFromToken(token) {
        const base64Payload = token.split('.')[1]
        const base64 = base64Payload.replace(/-/g, '+').replace(/_/g, '/')
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split('')
                .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        )
        return JSON.parse(jsonPayload)
    }
    

saveSimple.addEventListener("click",async(e)=>{
    const fName = document.getElementById("fName").value
    const petSpecies = document.getElementById("fSpecies").value
    const fBreed = document.getElementById("fBreed").value
    const petAge = document.getElementById("fAge").value
    const petWeight = document.getElementById("fWeight").value
    const petPorte = document.getElementById("fPorte").value
    const petSex = document.getElementById("fSex").value
    const petCastrated = document.getElementById("fCastrado").value

    const div_msg = document.getElementById("div_msg")

    const token = localStorage.getItem("token")
    if (!token) {
        div_msg.textContent = "Usuário não autenticado!"
        div_msg.style.color = "red"
        return
    }

    const payload = getPayloadFromToken(token)
    const userId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]

    if(fName==""||petSpecies==""||petAge==""||petWeight==""||petSex==""||fBreed==""||petCastrated==""||petPorte==""){
        div_msg.textContent = "Prencha todos os campos"
        div_msg.style.color = "red"
        setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
        return
    }

    try {
        const req = await fetch("http://localhost:5280/api/Animal/register",{
            method:"Post",
            headers:{
                "Content-Type":"Application/json",
                "Authorization": `Bearer ${token}`
            },
            body:JSON.stringify({Nome:fName,Especie:petSpecies,Raca:fBreed,Idade:petAge,Peso:petWeight,Porte:petPorte,ResponsavelId:[userId]})
        })
        const res = await req.json()
        console.log(res)
    } catch (error) {
        div_msg.textContent = "Erro interno"
        div_msg.style.color = "red"
        setTimeout(()=>{
            div_msg.textContent = ""
        },2000)
        return
    }
})