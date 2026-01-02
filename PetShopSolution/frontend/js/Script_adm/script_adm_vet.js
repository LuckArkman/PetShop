//Chamar os modais
const modal_vet = document.getElementById("backdropSimplevet")
//fim
//chamar os form
const form_vet = document.getElementById("form_vet")
//fim
//chamar btn add
const btn_add_vet = document.getElementById("btn_add_vet")
//fim
//chamar btn cancel
const btn_cancel_vet = document.getElementById("cancelSimplevet")
//fim
function modal(btn_add,modal,btn_cancel,form){
    console.log("ola")
    btn_add.addEventListener("click",(e)=>{
        console.log("ola")
        modal.style.display = "flex"
        form.reset()
    })
    btn_cancel.addEventListener("click",(e)=>{
        modal.style.display = "none"
    })
}

//chamar a rota
modal(btn_add_vet,modal_vet,btn_cancel_vet,form_vet)
//fim

function showMessage(message, color = "black", duration = 2000) {
        let toast = document.getElementById("toastMessage")
        if (!toast) {
            toast = document.createElement("div")
            toast.id = "toastMessage"
            document.body.appendChild(toast)
        }
        toast.textContent = message
        toast.style.color = color
        toast.style.display = "block"
        toast.style.opacity = "1"
      
        setTimeout(() => {
            toast.style.opacity = "0"
            setTimeout(() => {
                toast.style.display = "none"
            }, 300)
        }, duration)
}

const btn_cad = document.getElementById("saveSimplevet")

    btn_cad.addEventListener("click",async (e)=>{
        e.preventDefault()
        const fCmrv = document.getElementById("fCmrv").value
        const fEspvet = document.getElementById("fEspvet").value
        const username_user = document.getElementById("fNamevet").value
        const email_user = document.getElementById("fEmailvet").value
        const fTelvet = document.getElementById("fTelvet").value
        const senha_user = document.getElementById("fsenhavet").value
        const confirm_senha_user = document.getElementById("fConsenhavet").value
        if(senha_user!=confirm_senha_user){
            showMessage("Senhas não coincidem","red")
            return
        }
        if(username_user==""||email_user==""||senha_user==""||confirm_senha_user=="" || fCmrv=="" || fEspvet=="" || fTelvet==""){
            showMessage("Preencha todos os campos","red")
            return
        }
        try {
            const req = await fetch("https://petrakka.com:7231/api/MedicoVeterinario/register",{
                method:"Post",
                headers:{"Content-Type":"application/json"},
                body:JSON.stringify({Nome:username_user,CRMV:fCmrv,Password:senha_user,ConfirmPassword:confirm_senha_user,Telefone:fTelvet,Especialidade:fEspvet,Email:email_user})
            })
            const res = await req.json()
            console.log(res)
            if(req.status === 401){
                showMessage(res.message,"red")
            }
            if(res.id){
                showMessage("Cadastro realizado com sucesso","green")
            }else{
                showMessage(res.message,"red")
                return
            }
            
            
        } catch (error) {
            showMessage("Erro interno","red")
            return
        }
    })