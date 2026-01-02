//Chamar os modais
const modal_consul = document.getElementById("backdropSimpleconsul")
//fim
//chamar os form
const form_consul = document.getElementById("form_consul")
//fim
//chamar btn add
const btn_add_consul = document.getElementById("btn_add_consul")
//fim

//chamar btn cancel
const btn_cancel_consul = document.getElementById("cancelSimpleconsul")
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
modal(btn_add_consul,modal_consul,btn_cancel_consul,form_consul)
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
        const fCmrvconsul = document.getElementById("fCmrvconsul").value
        const rg_user = document.getElementById("fRgrec").value
        const data_consul = document.getElementById("data_consul").value
        const horario_consul = document.getElementById("horario_consul").value
        let data_formatada_back = data_consul+"T"+horario_consul+":00.000Z"
        if(fCmrvconsul=="" || rg_user==""){
            showMessage("Preencha todos os campos","red")
            return
        }
        const body_consulta = {
            id:"",
            animalId:div_pet_itens.id,
            rg:rg_tutor,
            crmv:cmrv_vet,
            dataConsulta:data_formatada_back,
            status:1    
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