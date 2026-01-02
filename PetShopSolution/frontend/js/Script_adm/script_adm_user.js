//Chamar os modais
const modal_user = document.getElementById("backdropSimpleuser")
//fim
//chamar os form
const form_user = document.getElementById("form_user")
//fim
//chamar btn add
const btn_add_user = document.getElementById("btn_add_user")
//fim

//chamar btn cancel
const btn_cancel_user = document.getElementById("cancelSimpleuser")
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
modal(btn_add_user,modal_user,btn_cancel_user,form_user)
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

const btn_cad = document.getElementById("saveSimpleuser")

    btn_cad.addEventListener("click",async (e)=>{
        e.preventDefault()
        const username_user = document.getElementById("fNameuser").value
        const email_user = document.getElementById("fEmailuser").value
        const senha_user = document.getElementById("fsenhauser").value
        const confirm_senha_user = document.getElementById("fConsenhauser").value
        const fCpfuser = document.getElementById("fCpfuser").value
        const rg_user = document.getElementById("fRguser").value
        if(senha_user!=confirm_senha_user){
            showMessage("Senhas não coincidem","red")
            return
        }
        if(username_user==""||email_user==""||senha_user==""||confirm_senha_user=="" || rg_user=="" || fCpfuser==""){
            showMessage("Preencha todos os campos","red")
            return
        }
        try {
            const req = await fetch("https://petrakka.com:7231/api/Responsavel/register",{
                method:"Post",
                headers:{"Content-Type":"application/json"},
                body:JSON.stringify({Email:email_user,Password:senha_user,ConfirmPassword:confirm_senha_user,FirstName:username_user,RG:rg_user,Cpf:fCpfuser})
            })
            const res = await req.json()
            console.log(res)
            if(req.status === 401){
                showMessage(res.message,"red")
            }
            if(res.message){
                showMessage("Cadastro realizado com sucesso","green")
            }else{
                showMessage("Erro ao se cadastrar","red")
                return
            }
            
            
        } catch (error) {
            showMessage("Erro interno","red")
            return
        }
    })