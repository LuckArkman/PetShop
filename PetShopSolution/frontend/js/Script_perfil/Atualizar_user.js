const btn_save = document.getElementById("btn_save")
const div_msg = document.getElementById("div_msg")

/*remove_user.addEventListener("click",async()=>{
    try {
        const req = fetch(`http://localhost:5280/api/Responsavel/delete?mail=${userEmail}`,{
            method:"DELETE"
        })
        const res =  req.json()
        if(res.Id){
            alert("Conta deletada com sucesso")
        }else{
            alert("Erro ao deletar conta")
        }
       } catch (error) {
            alert("Erro interno")
       }
    })*/

function getPayloadFromToken(token) {
    const base64Payload = token.split(".")[1]
    const base64 = base64Payload.replace(/-/g, "+").replace(/_/g, "/")
    const jsonPayload = decodeURIComponent(
        atob(base64)
            .split("")
            .map(c => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
            .join("")
    )
    return JSON.parse(jsonPayload)
}

const token = localStorage.getItem("token")
if (!token) {
    div_msg.textContent = "Usuário não autenticado!"
    div_msg.style.color = "red"
    throw new Error("Usuário não autenticado")
}
const payload = getPayloadFromToken(token)
const userId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
const userName = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]
console.log(userName)

btn_save.addEventListener("click",async(e)=>{
    e.preventDefault()
    alert("clicado")
    const update_first_name = document.getElementById("update_first_name").value
    const update_last_name = document.getElementById("update_last_name").value
    const update_email = document.getElementById("update_email").value
    const update_phone_number = document.getElementById("update_phone_number").value
    const update_cpf = document.getElementById("update_cpf").value
    const update_rg = document.getElementById("update_rg").value
    const update_password = document.getElementById("update_password").value
    const update_confirm_password = document.getElementById("update_confirm_password").value
    const update_address = document.getElementById("update_address").value
    const update_city = document.getElementById("update_city").value
    const update_state = document.getElementById("update_state").value
    const update_zip_code = document.getElementById("update_zip_code").value

    const updatedUser = {
        Id: userId,
        Email:update_email,
        FirstName:update_first_name,
        LastName:update_last_name,
        CPF:update_cpf,
        RG:update_rg,
        Password:update_password ,
        ConfirmPassword:update_confirm_password,
        Address:update_address ,
        City:update_city,
        State:update_state,
        ZipCode:update_zip_code,
        PhoneNumber:update_phone_number
    }

    try {
        const req = await fetch(`http://localhost:5280/api/Responsavel/update`,{
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(updatedUser)
        })
        const res = await req.json()
        if (res.Message) {
            div_msg.textContent = "Usuário atualizado com sucesso!"
            div_msg.style.color = "green"
            setTimeout(() => { div_msg.textContent = "" }, 2000)
        } else {
            div_msg.textContent = "Erro ao atualizar!"
            div_msg.style.color = "red"
            setTimeout(() => { div_msg.textContent = "" }, 2000)
        }
    } catch (error) {
        div_msg.textContent = "Erro interno!"
        div_msg.style.color = "red"
        setTimeout(() => { div_msg.textContent = "" }, 2000)
    }
})