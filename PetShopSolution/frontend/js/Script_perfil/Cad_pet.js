const btn_add_pet = document.getElementById("btn_add_pet")
const btn_remove_pet = document.getElementById("btn_remove_pet")
const backdropSimple = document.getElementById("backdropSimple")
const cancelSimple = document.getElementById("cancelSimple")
const saveSimple = document.getElementById("saveSimple")
const select_pet = document.getElementById("select_pet")
const div_msg = document.querySelectorAll(".div_msg")
const info_bottom_pet = document.getElementById("info_bottom_pet")
const btn_perfil = document.getElementById("btn_perfil")
const config_user = document.getElementById("config_user")
const sair_user = document.getElementById("sair_user")
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

document.addEventListener('DOMContentLoaded', async() => {
    const selectedPetId = JSON.parse(localStorage.getItem("selectedPetId"))
    if(!selectedPetId){
        return
    }
    Fetch_vacinas(selectedPetId)
    Fetch_diag(selectedPetId)
    Fetch_med(selectedPetId)
    Fetch_cir(selectedPetId)
})

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
btn_add_pet.addEventListener("click", () => backdropSimple.style.display = "flex")
cancelSimple.addEventListener("click", () => backdropSimple.style.display = "none")
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
    showMessage("Usuário não autenticado!", "red")
    setTimeout(() => window.location.href = "../../pages/pages_login/Login_user.html", 1000)
}
const payload = getPayloadFromToken(token)
const userId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
const userEmail = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"]
console.log(userEmail)
let avatar_img = document.getElementById("avatar_img")
function updatePetUI(pet) {
    document.getElementById("animal_name").textContent = pet.nome
    document.getElementById("race_animal").textContent = pet.raca || "Sem Raça"
    document.getElementById("petSpecies").textContent = pet.especie
    document.getElementById("petAge").textContent = pet._idade.anos>0?`${pet._idade.anos}anos`:`${pet._idade.meses}meses`
    document.getElementById("petWeight").textContent = pet._peso.kilos>0?`${pet._peso.kilos}kg`:`${pet._peso.gramas}gramas`
    document.getElementById("petSex").textContent = pet.sexo
    document.getElementById("petPorte").textContent = pet.porte
    document.getElementById("petCastrated").textContent = pet.castrado ? "Sim" : "Não"
    const avatar = document.getElementById("avatar")
    avatar.style.background = pet.sexo === "Fêmea" ? "Pink" : "linear-gradient(135deg,#3b82f6,#60a5fa)"
    avatar_img.src = pet.picture
}
const avatar = document.getElementById("avatar")


async function populatePets() {
    select_pet.innerHTML = `<option value="">Pet Atual</option>`
        try {
            const res = await fetch(`https://petrakka.com:7231/api/Responsavel/animais`, {
               method:"POST",
               headers:{"Content-Type":"application/json"},
               body:JSON.stringify({mail:userEmail})
            })
            console.log(res)
            const pets = await res.json()
            console.log(pets)
            const selectedPetId = localStorage.getItem("selectedPetId") 
            pets.forEach(pet => {
                const option = document.createElement("option")
                option.value = pet.id
                option.textContent = pet.nome
                if (selectedPetId === pet.id) {
                    option.selected = true
                    updatePetUI(pet)
                    info_bottom_pet.style.display = "block"
                }
                select_pet.appendChild(option)
            })
        } catch (err) {
            console.log(err)
        }
}

select_pet.addEventListener("change", async () => {
    const selectedId = select_pet.value
    if (!selectedId) return
    try {
        const res = await fetch(`https://petrakka.com:7231/api/Animal/animal/${selectedId}`, {
            headers: { "Authorization": `Bearer ${token}`}
        })
        const pet = await res.json()
        updatePetUI(pet)
        info_bottom_pet.style.display = "block"
        localStorage.setItem("selectedPetId", pet.id)
        let animalIds = JSON.parse(localStorage.getItem("userAnimalIds")) || []
        if (!animalIds.includes(pet.id)) {
        animalIds.push(pet.id)
        localStorage.setItem("userAnimalIds", JSON.stringify(animalIds))
        }
        const selectedPetId = localStorage.getItem("selectedPetId") 
        Fetch_vacinas(selectedPetId)
        Fetch_diag(selectedPetId)
        Fetch_med(selectedPetId)
        Fetch_cir(selectedPetId)
    } catch (err) {
        console.error("Erro ao buscar pet selecionado:", err)
    }
})

saveSimple.addEventListener("click", async () => {
    const fRg = document.getElementById("fRg").value.replace(/[^\dXx]/g, "")
    const fName = document.getElementById("fName").value
    const fSpecies = document.getElementById("fSpecies").value
    const fBreed = document.getElementById("fBreed").value
    const fAge = Number(document.getElementById("fAge").value).toFixed(0)
    const fWeight = Number(document.getElementById("fWeight").value).toFixed(0)
    const fPorte = document.getElementById("fPorte").value
    const fSex = document.getElementById("fSex").value
    const fCastrado = document.getElementById("fCastrado").value
    const fAgeSelect = document.getElementById("fAgeSelect").value
    const fWeightSelect = document.getElementById("fWeightSelect").value
    const fotoInput = document.getElementById("fFoto")
    const fotoFile = fotoInput.files[0] || null
    const fotoUrl = await uploadToCloudinary(fotoFile)

    const value_fCastrado = fCastrado === "Sim"
    const token = localStorage.getItem("token")
    if (!token) {
        showMessage("Usuário não autenticado!","red")
        return
    }
    if(isNaN(fAge) || isNaN(fWeight)){
        showMessage("Dados inválidos digite números inteiro para idade e peso","red")
        return
    }
    const payload = getPayloadFromToken(token)
    const userId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]

    if (!fName || !fSpecies || !fBreed || !fAge || !fWeight || !fPorte || !fSex || !fCastrado || !fRg || !fAgeSelect || !fWeightSelect) {
        showMessage("Preencha todos os campos!","red")
        return
    }
    if(fAgeSelect==="Anos" && fAge>30){
        showMessage("Idade inválida digite uma idade menor que 31 anos","red")
        return
    }else if(fAgeSelect==="Meses" && fAge>12){
        showMessage("Idade inválida digite uma idade menor que 13 meses ","red")
        return 
    }

     if(fWeightSelect==="Kilos" && fWeight>150){
        showMessage("Peso inválido digite um peso menor que 150 kilos","red")
        return
    }else if(fWeightSelect==="Gramas" && fWeight>1000){
        showMessage("Peso inválido digite um peso menor que 1000 gramas sass","red")
        return 
    }

    if(!fotoInput.files || fotoInput.files.length === 0){
        showMessage("Selecione uma imagem para seu pet","red")
    }
    try {
        const req = await fetch("https://petrakka.com:7231/api/Animal/register", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                nome: fName,
                especie: fSpecies,
                picture:fotoUrl,
                raca: fBreed,
                sexo: fSex,
                castrado: value_fCastrado,
                _idade:{
                    Anos:fAgeSelect==="Anos"?fAge:0,
                    Meses:fAgeSelect==="Meses"?fAge:0
                },
                _peso:{
                    Kilos:fWeightSelect==="Kilos"?fWeight:0,
                    Gramas:fWeightSelect==="Gramas"?fWeight:0
                },
                porte: fPorte,
                responsaveis: [userId]
            })
        })

        const res = await req.json()
        console.log(res)
        if (res.id) {
            const avatarImg = document.getElementById("avatar_img")
            avatarImg.src = res.picture
            if (avatarImg) {
                avatarImg.src = res.picture
            } else {
                console.error("Elemento #avatar_img não encontrado no DOM")
            }
            console.log(avatarImg)           
            localStorage.setItem("selectedPetId", JSON.stringify(res.id))
            let animalIds = JSON.parse(localStorage.getItem("userAnimalIds")) || []
            animalIds.push(res.id)
            localStorage.setItem("userAnimalIds", JSON.stringify(animalIds))
            const option = document.createElement("option")
            option.value = res.id
            option.textContent = res.nome
            option.selected = true
            select_pet.appendChild(option)
            updatePetUI(res)
            info_bottom_pet.style.display = "block"
            backdropSimple.style.display = "none"
            resetForm()
            showMessage("Cadastro realizado com sucesso!","green")
            const req_get_user = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${fRg}`,{
                method:"GET"
            })
            const res_get_user = await req_get_user.json()
            res_get_user.Animais = animalIds
            //setTimeout(() => location.reload(), 800)
            const req_put_user = await fetch(`https://petrakka.com:7231/api/Responsavel/update`,{
                method:"PUT",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify(res_get_user)
            })
        } else {
            showMessage("Erro ao cadastrar pet!","red")
        }
    } catch (err) {
        showMessage("Erro interno!","red")
        console.log(err)
    }
})

async function uploadToCloudinary(file) {
    const formData = new FormData()
    formData.append("file", file)
    formData.append("upload_preset", "back_projeto_pet")

    const res = await fetch(
        "https://api.cloudinary.com/v1_1/dlb26geus/image/upload",
        {
            method: "POST",
            body: formData
        }
    )

    if (!res.ok) {
        showMessage("Selecione uma imagem para seu pet","red")
        return
    }

    const data = await res.json()

    // ✅ ESSA é a URL que você vai mandar pro backend
    return data.secure_url
}

function resetForm() {
    document.getElementById("fRg").value = ""
    document.getElementById("fName").value = ""
    document.getElementById("fSpecies").value = ""
    document.getElementById("fBreed").value = ""
    document.getElementById("fAge").value = ""
    document.getElementById("fWeight").value = ""
    document.getElementById("fPorte").value = ""
    document.getElementById("fSex").value = ""
    document.getElementById("fCastrado").value = ""
}

// Rota excluir pet
btn_remove_pet.addEventListener("click",async()=>{

    try {
        const req = await fetch(`https://petrakka.com:7231/api/Animal/delete?id=${select_pet.value}`,{
            method:"DELETE",
            headers:{
                "Content-Type": "application/json"
            }
        })

        const res = await req.json()

        if(res){
            select_pet.value = ""
            info_bottom_pet.style.display = "none"
            document.getElementById("animal_name").textContent = "—"
            document.getElementById("race_animal").textContent = "Sem Raça"
            document.getElementById("petSpecies").textContent = "—"
            document.getElementById("petAge").textContent = "—"
            document.getElementById("petWeight").textContent = "—"
            document.getElementById("petSex").textContent = "—"
            document.getElementById("petPorte").textContent = "—"
            document.getElementById("petCastrated").textContent = "—"
            document.getElementById("avatar").style.background = "—"          
            await populatePets()
            localStorage.removeItem("selectedPetId")
            showMessage("Pet excluído com sucesso!","green")
            setTimeout(() => location.reload(), 800)
        }else{
            showMessage("Erro ao deletar","red")
        }
    } catch (error) {
        showMessage("Erro interno","red")
    }
})
//Fim rota excluir pet

/*Rota Atualizar pet*/
const btnAttPet = document.getElementById("btn_att_pet")
const backdropUpdate = document.getElementById("backdropUpdate")
const cancelUpdate = document.getElementById("cancelUpdate")
const saveUpdate = document.getElementById("saveUpdate")

btnAttPet.addEventListener("click", async () => {
    const petId = select_pet.value
    if (!petId) return


    const res = await fetch(`https://petrakka.com:7231/api/Animal/animal/${petId}`, {
        method:"GET",
        headers: { "Authorization": `Bearer ${token}` }
    })
    const pet = await res.json()
    document.getElementById("updateName").value = pet.nome
    document.getElementById("updateSpecies").value = pet.especie
    document.getElementById("updateBreed").value = pet.raca
    document.getElementById("updateAge").value = pet._idade.anos>0?`${pet._idade.anos}`:`${pet._idade.meses}`
    document.getElementById("updateWeight").value = pet._peso.kilos>0?`${pet._peso.kilos}`:`${pet._peso.gramas}`
    document.getElementById("updatePorte").value = pet.porte
    document.getElementById("updateSex").value = pet.sexo
    document.getElementById("updateCastrado").value = pet.castrado ? "Sim" : "Não"

    backdropUpdate.style.display = "flex"
})

cancelUpdate.addEventListener("click", () => {
    backdropUpdate.style.display = "none"
})

saveUpdate.addEventListener("click", async (e) => {
    e.preventDefault()
    const petId = select_pet.value
    const updateAge = Number(document.getElementById("updateAge").value).toFixed(0)
    const updateWeight = Number(document.getElementById("updateWeight").value).toFixed(0)
    const fAgeSelectupdate = document.getElementById("fAgeSelectupdate").value
    const fWeightSelectupdate = document.getElementById("fWeightSelectupdate").value
    console.log(fAgeSelectupdate)
    console.log(fWeightSelectupdate)
    if(fAgeSelectupdate==="Selecione"){
        showMessage("Selecione uma idade valida, anos ou meses","red")
        return
    }
    if(fAgeSelectupdate==="Anos" && updateAge>30){
        showMessage("Idade inválida digite uma idade menor que 31 anos","red")
        return
    }else if(fAgeSelectupdate==="Meses" && updateAge>12){
        showMessage("Idade inválida digite uma idade menor que 13 meses ","red")
        return 
    }
    if(fWeightSelectupdate==="Selecione"){
        showMessage("Selecione um peso valido, kilos ou gramas","red")
        return
    }
     if(fWeightSelectupdate==="Kilos" && updateWeight>150){
        showMessage("Peso inválido digite um peso menor que 150 kilos","red")
        return
    }else if(fWeightSelectupdate==="Gramas" && updateWeight>1000){
        showMessage("Peso inválido digite um peso menor que 1000 gramas sass","red")
        return 
    }
    const updatedPet = {
        id: petId,
        Nome: document.getElementById("updateName").value,
        Especie: document.getElementById("updateSpecies").value,
        Raca: document.getElementById("updateBreed").value,
        _idade:{
            anos:fAgeSelectupdate==="Anos"?Number(updateAge):0,
            meses:fAgeSelectupdate==="Meses"?Number(updateAge):0
            },
        _peso:{
            kilos:fWeightSelectupdate==="Kilos"?Number(updateWeight):0,
            gramas:fWeightSelectupdate==="Gramas"?Number(updateWeight):0
            },
        Porte: document.getElementById("updatePorte").value,
        Sexo: document.getElementById("updateSex").value,
        Castrado: document.getElementById("updateCastrado").value === "Sim",
        responsaveis: [userId]
    }

    console.log(updatedPet)

    const req = await fetch(`https://petrakka.com:7231/api/Animal/update?id=${petId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(updatedPet)
    })
    console.log(req)
    const res = await req.json()
    console.log(res)
    if (res.id) {
        updatePetUI(res)
        backdropUpdate.style.display = "none"
        showMessage("Pet atualizado com sucesso!","green")
        const optionToUpdate = Array.from(select_pet.options).find(opt => opt.value === res.id)
        if (optionToUpdate) {
            optionToUpdate.textContent = res.nome
        }
    } else {
        showMessage("Erro ao atualizar pet!","red")
    }
})
/*Fim rota atualizar pet*/


populatePets()

/*Modal Mostrar vacina*/
const toggleVacinas = document.getElementById("toggleVacinas")
const vacinaCount = document.getElementById("vacinaCount")
const saveVacina = document.getElementById("saveVacina")
const backdropVacina = document.getElementById("backdropVacina")
const backdropVacinasView = document.getElementById("backdropVacinasView")
const listaVacinasView  = document.getElementById("lista_vacinas_view")
const closeVacinasView = document.getElementById("closeVacinasView")

document.getElementById("btn_add_vacina").addEventListener("click", () => {
    backdropVacina.style.display = "flex"
})

document.getElementById("cancelVacina").addEventListener("click", () => {
    backdropVacina.style.display = "none"
})
saveVacina.addEventListener("click",async() => {
    const vacinaCount = document.getElementById("vacinaCount")
    const listaVacinasView  = document.getElementById("lista_vacinas_view")
    const nome = document.getElementById("vacinaNome").value
    const data = document.getElementById("vacinaData").value
    const crmvVet = document.getElementById("crmvVet").value
    const relVet = document.getElementById("relVet").value
    const animal_id = localStorage.getItem("selectedPetId")
    if(!animal_id){
        showMessage("Criei um animal primeiro","red")
    }
    if (!nome || !data || !crmvVet || !relVet) {
        showMessage("Preencha todos os dados","red")
        return
    }

    const obj_add_vacina = {
        AnimalId: animal_id,
        _dataVacinacao: data,
        Tipo: nome,
        Relatorio: relVet,
        _veterinarioCRMV: crmvVet,
        responsaveis: [
          userId
        ]
      }
      try {
        const req = await fetch("https://petrakka.com:7231/register",{
            method:"Post",
            headers:{
                "Content-Type":"application/json",
                "Authorization": `Bearer ${token}`
            },
            body:JSON.stringify(obj_add_vacina)
        })
        const res = await req.json()
        if(res.id){
            const req_get_hist = await fetch(`https://petrakka.com:7231/historico?AnimalId=${animal_id}`,{
                method:"GET"
            })
            const res_get_hist = await req_get_hist.json()
            Render_count_vacina(res_get_hist.length,vacinaCount)
            Render_list(res_get_hist,animal_id,listaVacinasView)
            showMessage("Vacina criada com sucesso","green")
        }else{
            showMessage("Erro ao criar vacina","red")
        }
    } catch (error) {
        showMessage("Erro interno","red") 
    }
})
const btn_ver_mais_vacina = document.getElementById("btn_ver_mais_vacina")
btn_ver_mais_vacina.addEventListener("click", () => {
    backdropVacinasView.style.display = "flex"
    Render_count_vacina(listaVacinasView.children.length,vacinaCount)
})

// fechar modal de visualização
closeVacinasView.addEventListener("click", () => {
    backdropVacinasView.style.display = "none"
    Render_count_vacina(listaVacinasView.children.length,vacinaCount)
})

/*Fim modal mostrar vacina*/

// --- Diagnóstico --
const btnAddDiagnostico = document.getElementById('btn_add_diagnostico')
const backdropDiagnostico = document.getElementById('backdropDiagnostico')
const cancelDiagnostico = document.getElementById('cancelDiagnostico')
btnAddDiagnostico.addEventListener('click', () => backdropDiagnostico.style.display = 'flex')
cancelDiagnostico.addEventListener('click', () => backdropDiagnostico.style.display = 'none')
backdropDiagnostico.addEventListener('click', e => { if(e.target === backdropDiagnostico) backdropDiagnostico.style.display = 'none'; })
const spanDiagnostico = document.getElementById('diagnosticoCount')
const viewDiagnostico = document.getElementById('backdropDiagnosticoView')
const closeDiagnosticoView = document.getElementById('closeDiagnosticoView')
const lista_diagnosticos_view = document.getElementById("lista_diagnosticos_view")
const btn_ver_mais_diag = document.getElementById("btn_ver_mais_diag")
btn_ver_mais_diag.addEventListener('click', () => {
    viewDiagnostico.style.display = 'flex'
})

closeDiagnosticoView.addEventListener('click', () => {
    viewDiagnostico.style.display = 'none'
    Render_count_diag(lista_diagnosticos_view.children.length,spanDiagnostico)
})
viewDiagnostico.addEventListener('click', e => { if(e.target === viewDiagnostico) viewDiagnostico.style.display = 'none' })

const saveDiagnostico = document.getElementById("saveDiagnostico")
saveDiagnostico.addEventListener("click",async() => {
    const spanDiagnostico = document.getElementById('diagnosticoCount')
    const lista_diagnosticos_view = document.getElementById("lista_diagnosticos_view")
    const diagnosticoData = document.getElementById("diagnosticoData").value
    const diagnosticoCondicao = document.getElementById("diagnosticoCondicao").value
    const diagnosticoSintomas = document.getElementById("diagnosticoSintomas").value
    const diagnosticoExames = document.getElementById("diagnosticoExames").value
    const diagnosticoTratamento = document.getElementById("diagnosticoTratamento").value
    const animal_id = localStorage.getItem("selectedPetId")
    if(!animal_id){
        showMessage("Criei um animal primeiro","red")
    }
    if (!diagnosticoData || !diagnosticoCondicao || !diagnosticoSintomas || !diagnosticoExames || !diagnosticoTratamento) {
        showMessage("Preencha todos os dados","red")
        return
    }

    const obj_add_diag = {
        animalId: animal_id,
        _data: diagnosticoData,
        doencaOuCondicao: diagnosticoCondicao,
        sintomasObservados: diagnosticoSintomas,
        examesSolicitados: diagnosticoExames,
        condutaTratamento:diagnosticoTratamento
      }
      try {
        const req = await fetch("https://petrakka.com:7231/api/Diagnostico/register",{
            method:"Post",
            headers:{
                "Content-Type":"application/json",
                "Authorization": `Bearer ${token}`
            },
            body:JSON.stringify(obj_add_diag)
        })
        if (req.status === 204) {
            showMessage("Pet atualizado com sucesso!", "green")
            backdropUpdate.style.display = "none"
            await populatePets()
            return
        }
        const res = await req.json()
        if(res.id){
            const req_get_hist = await fetch(`https://petrakka.com:7231/api/Diagnostico/Diagnosticos?animalId=${animal_id}`,{
                method:"GET"
            })
            const res_get_hist = await req_get_hist.json()
            Render_count_diag(res_get_hist.length,spanDiagnostico)
            Render_list_diagnostico(res_get_hist,animal_id,lista_diagnosticos_view)
            showMessage("Diagnóstico criada com sucesso","green")
        }else{
            showMessage("Erro ao criar diagnóstico","red")
        }
    } catch (error) {
        showMessage("Erro interno","red") 
    }
})

// --- Medicação ---
const btnAddMedicamento = document.getElementById('btn_add_medicamento')
const backdropMedicamento = document.getElementById('backdropMedicamento')
const cancelMedicamento = document.getElementById('cancelMedicamento')
btnAddMedicamento.addEventListener('click', () => backdropMedicamento.style.display = 'flex')
cancelMedicamento.addEventListener('click', () => backdropMedicamento.style.display = 'none')
backdropMedicamento.addEventListener('click', e => { if(e.target === backdropMedicamento) backdropMedicamento.style.display = 'none' })
const spanMedicamento = document.getElementById('medicamentoCount')
const viewMedicamento = document.getElementById('backdropMedicamentoView')
const closeMedicamentoView = document.getElementById('closeMedicamentosView')
const lista_medicamentos_view = document.getElementById("lista_medicamentos_view")
const btn_ver_mais_med = document.getElementById("btn_ver_mais_med")
btn_ver_mais_med.addEventListener("click",()=>{
    viewMedicamento.style.display = 'flex' 
})
closeMedicamentoView.addEventListener('click', () => {
    viewMedicamento.style.display = 'none' 
    Render_count_med(lista_medicamentos_view.children.length,spanMedicamento) 
})
viewMedicamento.addEventListener('click', e => { if(e.target === viewMedicamento) viewMedicamento.style.display = 'none' })

const saveMedicamento = document.getElementById("saveMedicamento")
saveMedicamento.addEventListener("click",async() => {
    const medicamentoData = document.getElementById("medicamentoData").value
    const medicamentoNome = document.getElementById("medicamentoNome").value
    const medicamentoDosagem = document.getElementById("medicamentoDosagem").value
    const medicamentoDuracao = document.getElementById("medicamentoDuracao").value
    const medicamentoIndicacao = document.getElementById("medicamentoIndicacao").value
    const medicamentoReacoes = document.getElementById("medicamentoReacoes_o1").value
    const animal_id = localStorage.getItem("selectedPetId")
    if(!animal_id){
        showMessage("Criei um animal primeiro","red")
    }
    if (!medicamentoData || !medicamentoNome || !medicamentoDosagem || !medicamentoDuracao || !medicamentoIndicacao || !medicamentoReacoes) {
        showMessage("Preencha todos os dados","red")
        return
    }

    const obj_add_med = {
        animalId: animal_id,
        data: medicamentoData,
        nome: medicamentoNome,
        dosagem: medicamentoDosagem,
        duracao: medicamentoDuracao,
        indicacao:medicamentoIndicacao,
        observacoes:medicamentoReacoes
      }
      try {
        const req = await fetch("https://petrakka.com:7231/api/Medicacao/register",{
            method:"Post",
            headers:{
                "Content-Type":"application/json",
                "Authorization": `Bearer ${token}`
            },
            body:JSON.stringify(obj_add_med)
        })
        const res = await req.json()
        if(res.id){
            const req_get_hist = await fetch(`https://petrakka.com:7231/api/Medicacao/Medicacoes?animalId=${animal_id}`,{
                method:"GET"
            })
            const res_get_hist = await req_get_hist.json()
            Render_count_med(res_get_hist.length,spanMedicamento)
            Render_list_med(res_get_hist,animal_id,lista_medicamentos_view)
            showMessage("Medicamento criada com sucesso","green")
        }else{
            showMessage("Erro ao criar Medicamento","red")
        }
    } catch (error) {
        showMessage("Erro interno","red") 
    }
})
// --- Cirurgia ---
const btnAddCirurgia = document.getElementById('btn_add_cirurgia')
const backdropCirurgia = document.getElementById('backdropCirurgia')
const cancelCirurgia = document.getElementById('cancelCirurgia')
btnAddCirurgia.addEventListener('click', () => backdropCirurgia.style.display = 'flex')
cancelCirurgia.addEventListener('click', () => backdropCirurgia.style.display = 'none')
backdropCirurgia.addEventListener('click', e => { if(e.target === backdropCirurgia) backdropCirurgia.style.display = 'none'; })
const spanCirurgia = document.getElementById('cirurgiaCount')
const viewCirurgia = document.getElementById('backdropCirurgiaView')
const closeCirurgiaView = document.getElementById('closeCirurgiasView')
const lista_cirurgias_view = document.getElementById("lista_cirurgias_view")
const btn_ver_mais_cir = document.getElementById("btn_ver_mais_cir")
btn_ver_mais_cir.addEventListener('click', () => viewCirurgia.style.display = 'flex')
closeCirurgiaView.addEventListener('click', () => {
   viewCirurgia.style.display = 'none'
   Render_count_cir(lista_cirurgias_view.children.length,spanCirurgia) 
})
viewCirurgia.addEventListener('click', e => { if(e.target === viewCirurgia) viewCirurgia.style.display = 'none'; })
const saveCirurgia = document.getElementById("saveCirurgia")
saveCirurgia.addEventListener("click",async() => {
    const cirurgiaData = document.getElementById("cirurgiaData").value
    const tipoCirurgia = parseInt(document.getElementById("tipoCirurgia").value, 10)
    const cirurgiaMotivo = document.getElementById("cirurgiaMotivo").value
    const cirurgiaProcedimento = document.getElementById("cirurgiaProcedimento").value
    const cirurgiaPos = document.getElementById("cirurgiaPos").value
    const cirurgiaAlta = document.getElementById("cirurgiaAlta").value
    const relSintomas = document.getElementById("relSintomas").value
    const relTratamento = document.getElementById("relTratamento").value
    const relObservacoes = document.getElementById("relObservacoes").value
    const relVeterinario = document.getElementById("relVeterinario").value
    const animal_id = localStorage.getItem("selectedPetId")
    if(!animal_id){
        showMessage("Criei um animal primeiro","red")
    }
    if (!cirurgiaData || !tipoCirurgia || !cirurgiaMotivo || !cirurgiaProcedimento || !cirurgiaPos || !cirurgiaAlta || !relSintomas || !relTratamento || !relObservacoes || !relVeterinario) {
        showMessage("Preencha todos os dados","red")
        return
    }
    const obj_rel = {
        animalId:animal_id,
        _data:new Date().toISOString(),
        Sintomas:relSintomas,
        Tratamento:relTratamento,
        Observacoes:relObservacoes,
        VeterinarioId:relVeterinario
    }
    const obj_add_cir = {
        animalId: animal_id,
        data: cirurgiaData,
        tipo: tipoCirurgia,
        motivo: cirurgiaMotivo,
        procedimentoRealizado: cirurgiaProcedimento,
        relatorio:obj_rel,
        posOperatorioAcompanhamento:cirurgiaPos,
        dataAlta:cirurgiaAlta,
      }
      try {
        const req = await fetch("https://petrakka.com:7231/api/CirurgiaControllers/register",{
            method:"Post",
            headers:{
                "Content-Type":"application/json",
                "Authorization": `Bearer ${token}`
            },
            body:JSON.stringify(obj_add_cir)
        })
        const res = await req.json()


        if(res.id){
            const req_get_hist = await fetch(`https://petrakka.com:7231/api/CirurgiaControllers/Cirurgias?animalId=${animal_id}`,{
                method:"GET"
            })
            const res_get_hist = await req_get_hist.json()
            Render_count_cir(res_get_hist.length,spanCirurgia)
            Render_list_cir(res_get_hist,animal_id,lista_cirurgias_view)
            showMessage("Cirurgia criada com sucesso","green")
            resetFormCir()

        }else{
            showMessage("Erro ao criar cirurgia","red")
        }
    } catch (error) {
        showMessage("Erro interno","red") 

    }
})


function resetFormCir(){
    document.getElementById("cirurgiaData").value = ""
    parseInt(document.getElementById("tipoCirurgia").value = "", 10)
    document.getElementById("cirurgiaMotivo").value = ""
    document.getElementById("cirurgiaProcedimento").value = ""
    document.getElementById("cirurgiaPos").value = ""
    document.getElementById("cirurgiaAlta").value = ""
    document.getElementById("relSintomas").value = ""
    document.getElementById("relTratamento").value = ""
    document.getElementById("relObservacoes").value = ""
    document.getElementById("relVeterinario").value = ""
}


/*Funções*/
document.querySelectorAll(".modal-backdrop").forEach(b =>
    b.addEventListener("click", e=> { 
        if (e.target === b) b.style.display = "none" 
        if(b.id === "backdropVacinasView"){
            Render_count_vacina(listaVacinasView.children.length,vacinaCount)
        }
        if(b.id === "backdropDiagnosticoView"){
            Render_count_diag(lista_diagnosticos_view.children.length,spanDiagnostico)
        }
        if(b.id === "backdropMedicamentoView"){
            Render_count_diag(lista_medicamentos_view.children.length,spanMedicamento)
        }
    })
)

function Render_count_vacina(arr_length,element){
    element.innerText = `(${arr_length})`
}

function Render_list(res_get_hist,animal_id,lista_view){
    lista_view.innerHTML = ''
            res_get_hist.forEach((valor, index) => {   
                let data_format_arr = String(valor._dataVacinacao).slice(0,10).split("-")
                let data_format_br = new Date(data_format_arr[0]+"/"+data_format_arr[1]+"/"+data_format_arr[2]).toLocaleDateString("pt-BR")
                const item_vacina = document.createElement("div")
                item_vacina.classList.add("vacina_item")
                const div_info = document.createElement("div")
                div_info.classList.add("vacina_info")
                const btn_remove_itens = document.createElement("button")
                btn_remove_itens.innerText = "Excluir"
                btn_remove_itens.dataset.vacinaId = valor.id  
                btn_remove_itens.classList.add("btn_excluir")
                const itens_name = document.createElement("strong")
                const itens_rel = document.createElement("span")
                const itens_data = document.createElement("small")
                const itens_crmv = document.createElement("small")
                itens_name.innerText = `Nome: ${valor.tipo}`
                itens_rel.innerText = `Relatório: ${valor.relatorio}`
                itens_data.innerText = `Data: ${data_format_br}` 
                itens_crmv.innerText = `Crmv: ${valor._veterinarioCRMV}`
                div_info.appendChild(itens_name)
                div_info.appendChild(itens_rel)
                div_info.appendChild(itens_data)
                div_info.appendChild(itens_crmv)
                item_vacina.appendChild(div_info)
                item_vacina.appendChild(btn_remove_itens)
                lista_view.appendChild(item_vacina)
                btn_remove_itens.addEventListener("click", async () => {
                const vacinaId = btn_remove_itens.dataset.vacinaId
                try {
                const req_remove = await fetch(`https://petrakka.com:7231/remove?id=${vacinaId}`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify({
                    AnimalId: animal_id,
                    VacinaId: vacinaId
                    })
                })
                const res_remove = await req_remove.json()
                if (res_remove.vacinaId) {
                    item_vacina.remove()
                    showMessage("Vacina Deletada com sucesso","green")
                }else{
                    showMessage("Erro ao delelar vacina","red")
                }
                } catch (err) {
                    showMessage("Erro interno","red")
                }
                })
            })
}

async function Fetch_vacinas(animal_id){
    try {
     const req_get_hist = await fetch(`https://petrakka.com:7231/historico?AnimalId=${animal_id}`,{
         method:"GET"
     })
     if (!req_get_hist.ok) {
        Render_list([], animal_id, listaVacinasView)
        Render_count_vacina(0, vacinaCount)
        return
    }
     const res_get_hist = await req_get_hist.json()
     Render_list(res_get_hist,animal_id,listaVacinasView)
     Render_count_vacina(res_get_hist.length,vacinaCount)
    } catch (error) {

    }
 }

function Render_list_diagnostico(res_get_hist, animal_id, lista_view) {
    lista_view.innerHTML = ''
    res_get_hist.forEach(valor => {
        const item = document.createElement("div")
        item.classList.add("vacina_item")

        const div_info = document.createElement("div")
        div_info.classList.add("vacina_info")

        const btn_remove = document.createElement("button")
        btn_remove.innerText = "Excluir"
        btn_remove.dataset.id = valor.id
        btn_remove.classList.add("btn_excluir")

        const itens_cond = document.createElement("strong")
        itens_cond.innerText = `Condição: ${valor.doencaOuCondicao}`

        const itens_sint = document.createElement("span")
        itens_sint.innerText = `Sintomas: ${valor.sintomasObservados}`

        const itens_exam = document.createElement("small")
        itens_exam.innerText = `Exames: ${valor.examesSolicitados}`

        const itens_trat = document.createElement("small")
        itens_trat.innerText = `Tratamento: ${valor.condutaTratamento}`

        const itens_data = document.createElement("small")
        itens_data.innerText = `Data: ${valor._data}`

        div_info.append(itens_cond, itens_sint, itens_exam, itens_trat, itens_data)
        item.append(div_info, btn_remove)
        lista_view.appendChild(item)

        btn_remove.addEventListener("click", async () => {
            const registroId = btn_remove.dataset.id

            try {
                const req_remove = await fetch(`https://petrakka.com:7231/api/Diagnostico/delete?id=${registroId}`, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify({
                        AnimalId: animal_id,
                        id: registroId
                    })
                })
                const res_remove = await req_remove.json()
                if (res_remove.id) {
                    item.remove()
                    showMessage("Diagnóstico deletado com sucesso","green")
                } else {
                    showMessage("Erro ao deletar diagnóstico", "red")
                }
            } catch (err) {
                showMessage("Erro interno", "red")
            }
        })
    })
}


function Render_count_diag(arr_length,element){
    element.innerText = `(${arr_length})`
}

async function Fetch_diag(animal_id){
    try {
     const req_get_hist = await fetch(`https://petrakka.com:7231/api/Diagnostico/Diagnosticos?animalId=${animal_id}`,{
         method:"GET"
     })
     const res_get_hist = await req_get_hist.json()
     Render_list_diagnostico(res_get_hist,animal_id,lista_diagnosticos_view)
     
     Render_count_diag(res_get_hist.length,spanDiagnostico)
    } catch (error) {

    }
 }

 function Render_list_med(res_get_hist, animal_id, lista_view) {
    lista_view.innerHTML = ''
    res_get_hist.forEach(valor => {
        const item = document.createElement("div")
        item.classList.add("vacina_item")

        const div_info = document.createElement("div")
        div_info.classList.add("vacina_info")

        const btn_remove = document.createElement("button")
        btn_remove.innerText = "Excluir"
        btn_remove.dataset.id = valor.id
        btn_remove.classList.add("btn_excluir")

        const itens_cond = document.createElement("strong")
        itens_cond.innerText = `Medicamento: ${valor.nome}`

        const itens_sint = document.createElement("span")
        itens_sint.innerText = `Dosagem: ${valor.dosagem}`

        const itens_exam = document.createElement("small")
        itens_exam.innerText = `Duração: ${valor.duracao}`

        const itens_trat = document.createElement("small")
        itens_trat.innerText = `Indicação: ${valor.indicacao}`

        const itens_obs = document.createElement("small")
        itens_obs.innerText = `Observações: ${valor.observacoes}`

        const itens_data = document.createElement("small")
        itens_data.innerText = `Data: ${valor.data}`

        div_info.append(itens_cond, itens_sint, itens_exam, itens_trat, itens_data)
        item.append(div_info, btn_remove)
        lista_view.appendChild(item)

        btn_remove.addEventListener("click", async () => {
            const registroId = btn_remove.dataset.id
            try {
                const req_remove = await fetch(`https://petrakka.com:7231/api/Medicacao/delete?id=${registroId}`, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify({
                        AnimalId: animal_id,
                        id: registroId
                    })
                })
                const res_remove = await req_remove.json()
                if (res_remove.id) {
                    item.remove()
                    showMessage("Medicamento deletado com sucesso", "red")
                } else {
                    showMessage("Erro ao deletar Medicamento", "red")
                }
            } catch (err) {
                showMessage("Erro interno", "red")
            }
        })
    })
}


function Render_count_med(arr_length,element){
    element.innerText = `(${arr_length})`
}

async function Fetch_med(animal_id){
    try {
     const req_get_hist = await fetch(`https://petrakka.com:7231/api/Medicacao/Medicacoes?animalId=${animal_id}`,{
         method:"GET"
     })
     const res_get_hist = await req_get_hist.json()
     Render_list_med(res_get_hist,animal_id,lista_medicamentos_view)
     
     Render_count_diag(res_get_hist.length,spanMedicamento)
    } catch (error) {

    }
 }


 function Render_list_cir(res_get_hist, animal_id, lista_view) {
    lista_view.innerHTML = ''
    res_get_hist.forEach(valor => {
        const item = document.createElement("div")
        item.classList.add("vacina_item")

        const div_info = document.createElement("div")
        div_info.classList.add("vacina_info")

        const btn_remove = document.createElement("button")
        btn_remove.innerText = "Excluir"
        btn_remove.dataset.id = valor.id
        btn_remove.classList.add("btn_excluir")

        const itens_cond = document.createElement("strong")
        itens_cond.innerText = `motivo: ${valor.motivo}`

        const itens_sint = document.createElement("small")
        itens_sint.innerText = `Pós operatório: ${valor.posOperatorioAcompanhamento}`

        const itens_exam = document.createElement("small")
        itens_exam.innerText = `Procedimento: ${valor.procedimentoRealizado}`

        const itens_data = document.createElement("small")
        itens_data.innerText = `Data: ${valor.data}`
        const itens_data_alta = document.createElement("small")
        itens_data_alta.innerText = `Alta: ${valor.dataAlta}`
        const hr_rel = document.createElement("hr")
        const itens_rel = document.createElement("strong")
        itens_rel.innerText = "Relatório médico:"
        const itens_sin_rel = document.createElement("small")
        itens_sin_rel.innerText = `Sintomoas: ${valor.relatorio.Sintomas}`
        const itens_trat_rel = document.createElement("small")
        itens_trat_rel.innerText = `Tratamento: ${valor.relatorio.Tratamento}`
        const itens_obs_rel = document.createElement("small")
        itens_obs_rel.innerText = `Observações: ${valor.relatorio.Observacoes}`
        const itens_data_rel = document.createElement("small")
        itens_data_rel.innerText = `Data: ${valor.relatorio._data}`
        div_info.append(itens_cond, itens_sint, itens_exam,itens_data,itens_data_alta,hr_rel,itens_rel,itens_sin_rel,itens_trat_rel,itens_obs_rel,itens_data_rel)
        item.append(div_info, btn_remove)
        lista_view.appendChild(item)

        btn_remove.addEventListener("click", async () => {
            const registroId = btn_remove.dataset.id
            try {
                const req_remove = await fetch(`https://petrakka.com:7231/api/CirurgiaControllers/delete?id=${registroId}`, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify({
                        AnimalId: animal_id,
                        id: registroId
                    })
                })
                const res_remove = await req_remove.json()
                if (res_remove.id) {
                    item.remove()
                    showMessage("Cirurgia deletada com sucesso", "green")
                } else {
                    showMessage("Erro ao deletar cirurgia", "red")
                }
            } catch (err) {
                showMessage("Erro interno", "red")
            }
        })
    })
}


function Render_count_cir(arr_length,element){
    element.innerText = `(${arr_length})`
}

async function Fetch_cir(animal_id){
    try {
     const req_get_hist = await fetch(`https://petrakka.com:7231/api/CirurgiaControllers/Cirurgias?animalId=${animal_id}`,{
         method:"GET"
     })
     const res_get_hist = await req_get_hist.json()
     Render_list_cir(res_get_hist,animal_id,lista_cirurgias_view)
     Render_count_cir(res_get_hist.length,spanCirurgia)
    } catch (error) {

    }
 }
 