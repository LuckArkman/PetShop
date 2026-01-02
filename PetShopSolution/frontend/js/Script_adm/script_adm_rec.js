//Chamar os modais
const modal_rec = document.getElementById("backdropSimplerec")
//fim
//chamar os form
const form_rec = document.getElementById("form_recep")
//fim
//chamar btn add
const btn_add_rec = document.getElementById("btn_add_rec")
//fim

//chamar btn cancel
const btn_cancel_rec = document.getElementById("cancelSimplerec")
//fim
function modal(btn_add,modal,btn_cancel,form){
    btn_add.addEventListener("click",(e)=>{
        modal.style.display = "flex"
        form.reset()
    })
    btn_cancel.addEventListener("click",(e)=>{
        modal.style.display = "none"
    })
}

//chamar a rota
modal(btn_add_rec,modal_rec,btn_cancel_rec,form_rec)
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

const btn_cad = document.getElementById("saveSimplerec")
    btn_cad.addEventListener("click",async (e)=>{
        e.preventDefault()
        const username_user = document.getElementById("fNamerec").value
        const email_user = document.getElementById("fEmailrec").value
        const senha_user = document.getElementById("fsenharec").value
        const confirm_senha_user = document.getElementById("fConsenharec").value
        const rg_user = document.getElementById("fRgrec").value
        if(senha_user!=confirm_senha_user){
            showMessage("Senhas não coincidem","red")
            return
        }
        if(username_user==""||email_user==""||senha_user==""||confirm_senha_user=="" || rg_user==""){
            showMessage("Preencha todos os campos","red")
            return
        }
        try {
            const req = await fetch("https://petrakka.com:7231/api/Atendente/register",{
                method:"Post",
                headers:{"Content-Type":"application/json"},
                body:JSON.stringify({Email:email_user,Password:senha_user,ConfirmPassword:confirm_senha_user,FirstName:username_user,RG:rg_user})
            })
            const res = await req.json()
            console.log(res)
            if(req.status === 401){
                showMessage(res.message,"red")
            }
            if(res.user){
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