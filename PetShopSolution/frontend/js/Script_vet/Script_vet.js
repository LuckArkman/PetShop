const name_pet = document.getElementById("name_pet")
const name_tutor = document.getElementById("name_tutor")
const option_esp = document.getElementById("option_esp")
const type_atend = document.getElementById("type_atend")
const date_atend = document.getElementById("date_atend")
const value_diag = document.getElementById("value_diag")
const type_presc = document.getElementById("type_presc")
const return_diag = document.getElementById("return_diag")
const btn_save = document.getElementById("btn_save")
const search_results = document.getElementById("search_resultsVet")
const search_results_t = document.getElementById("search_resultsTutor")

let selectedPetId = null
let selectedTutorName = ""
let selectedHorario = ""

function resetForm(){
    name_pet.value = ""
    name_tutor.value = ""
    option_esp.value = ""
    type_atend.value = ""
    date_atend.value = ""
    value_diag.value = ""
    type_presc.value = ""
    return_diag.value = ""
    selectedPetId = null
    selectedTutorName = ""
    selectedHorario = ""
}

document.addEventListener("DOMContentLoaded",async e=>{
    const body_consul_day = document.getElementById("body_consul_day")
    try{
        const req = await fetch("https://petrakka.com:7231/api/Agendamento/hoje",{method:"GET"})
        const res = await req.json()
        for(let i=0;i<res.length;i++){
            if(res[i].id){
                const req_pet = await fetch(`https://petrakka.com:7231/api/Animal/animal/${res[i].animalId}`,{method:"GET"})
                const res_pet = await req_pet.json()
                if(res_pet.id){
                    const req_tutor = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${res[i].rg}`,{method:"GET"})
                    const res_tutor = await req_tutor.json()
                    let horario_form = res[i].dataConsulta.slice(11,16)
                    let tr_body_itens = document.createElement("tr")
                    let td_horario = document.createElement("td")
                    let td_nome_pet = document.createElement("td")
                    let td_nome_tutor = document.createElement("td")
                    let stats = document.createElement("span")
                    let btn_cancel = document.createElement("button")
                    btn_cancel.innerText = "Cancelar consulta"
                    btn_cancel.classList.add("cancel-btn")
                    td_horario.innerText = horario_form
                    td_nome_pet.innerText = res_pet.nome
                    td_nome_tutor.innerText = res_tutor.firstName

                    if(res[i].status == 0){
                        stats.classList.add("status","status-pending")
                        stats.innerText = "Pendente"
                    }else if(res[i].status == 1){
                        stats.classList.add("status","status-progress")
                        stats.innerText = "Em andamento"
                    }else{
                        stats.classList.add("status","status-done")
                        stats.innerText = "Concluída"
                    }

                    tr_body_itens.appendChild(td_horario)
                    tr_body_itens.appendChild(td_nome_pet)
                    tr_body_itens.appendChild(td_nome_tutor)
                    tr_body_itens.appendChild(stats)
                    tr_body_itens.appendChild(btn_cancel)
                    body_consul_day.appendChild(tr_body_itens)

                    stats.addEventListener("click",async e=>{
                        name_pet.value = td_nome_pet.innerText
                        name_tutor.value = td_nome_tutor.innerText
                        selectedPetId = res_pet.id
                        selectedHorario = td_horario.innerText

                        if(stats.classList[1] === "status-done"){
                            showMessage("Consulta já concluida","red")
                            return
                        }

                        try{
                            await fetch(`https://petrakka.com:7231/api/Agendamento/${res[i].id}/status?status=1`,{
                                method:"PATCH",
                                headers:{"Content-Type":"application/json"}
                            })
                            stats.classList.remove("status-pending","status-done")
                            stats.classList.add("status-progress")
                            stats.innerText = "Em andamento"
                        }catch(error){}

                        btn_save.onclick = async ()=>{
                            let data_formatada = date_atend.value+"T"+selectedHorario+":00.000"
                            const obj_dados_rel = {
                                animalId:selectedPetId,
                                _data:data_formatada,
                                Sintomas:value_diag.value,
                                Tratamento:type_presc.value,
                                Observacoes:type_atend.value,
                                VeterinarioId:""
                            }

                            try{
                                const req_rel = await fetch("https://petrakka.com:7231/api/RelatorioClinico/register",{
                                    method:"POST",
                                    headers:{"Content-Type":"application/json"},
                                    body:JSON.stringify(obj_dados_rel)
                                })
                                const res_rel = await req_rel.json()

                                if(res_rel.id){
                                    await fetch(`https://petrakka.com:7231/api/Agendamento/${res[i].id}/status?status=2`,{
                                        method:"PATCH",
                                        headers:{"Content-Type":"application/json"}
                                    })

                                    stats.classList.remove("status-progress")
                                    stats.classList.add("status-done")
                                    stats.innerText = "Concluída"

                                    showMessage("consulta realizada com sucesso","green")
                                    resetForm()
                                }
                            }catch(error){}
                        }
                    })

                    btn_cancel.addEventListener("click", async e => {
                        if(stats.classList[1] === "status-done"){
                            showMessage("Consulta já concluida, não pode ser cancelada", "red")
                            return
                        }
                    
                        if(stats.classList[1] === "status-pending"){
                            showMessage("Consulta pendente, não pode ser cancelada", "red")
                            return
                        }
                    
                        try{
                            await fetch(`https://petrakka.com:7231/api/Agendamento/${res[i].id}/status?status=0`, {
                                method:"PATCH",
                                headers:{
                                    "Content-Type":"application/json"
                                }
                            })
                    
                            stats.classList.remove("status-progress", "status-done")
                            stats.classList.add("status-pending")
                            stats.innerText = "Pendente"
                    
                            if(stats.classList[1] === "status-pending"){
                                showMessage("Consulta cancelada com sucesso", "green")
                            } else {
                                showMessage("Erro ao cancelar consulta", "red")
                            }
                        }catch(error){
                            console.error("Erro ao cancelar consulta:", error)
                        }
                    })
                    
                }
            }
        }
    }catch(error){}
})

function showMessage(message,color="black",duration=2000){
    let toast = document.getElementById("toastMessage")
    if(!toast){
        toast = document.createElement("div")
        toast.id = "toastMessage"
        document.body.appendChild(toast)
    }
    toast.textContent = message
    toast.style.color = color
    toast.style.display = "block"
    toast.style.opacity = "1"
    setTimeout(()=>{
        toast.style.opacity="0"
        setTimeout(()=>{
            toast.style.display="none"
        },300)
    },duration)
}

const btn_search = document.getElementById("btn_search")
btn_search.addEventListener("click",async e=>{
    const value_rg = document.getElementById("value_rg").value
    if(value_rg===""){
        showMessage("Preencha corretamente","red")
        return
    }
    try{
        const req = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${value_rg}`,{method:"GET"})
        const res = await req.json()
        tutor_pet(res)
    }catch(error){}
})

async function tutor_pet(res){
    const resultado_tutor_container = document.getElementById("resultado_tutor_container")
    resultado_tutor_container.style.display = "block"
    const pet_lista = document.getElementById("pet_lista")
    pet_lista.innerHTML = ""
    const spantutor_name = document.getElementById("spantutor_name")
    spantutor_name.textContent = res.firstName
    selectedTutorName = res.firstName

    for(const itens of res.animais){
        const div_pet_itens = document.createElement("div")
        div_pet_itens.classList.add("pet-item")
        const pet_icone = document.createElement("span")
        pet_icone.classList.add("pet-icone")
        pet_icone.textContent = "🐾"
        const name_pet_item = document.createElement("p")
        const btn_pet_select = document.createElement("button")
        btn_pet_select.textContent = "Selecionar"
        btn_pet_select.classList.add("btn_info_user")

        btn_pet_select.onclick = async()=>{
            selectedPetId = itens
            const pet = await fetch(`https://petrakka.com:7231/api/Animal/animal/${itens}`).then(r=>r.json())
            name_pet.value = pet.nome
            name_tutor.value = selectedTutorName
            showMessage("Pet selecionado para consulta","green")
        }

        div_pet_itens.appendChild(pet_icone)
        div_pet_itens.appendChild(name_pet_item)
        div_pet_itens.appendChild(btn_pet_select)
        pet_lista.appendChild(div_pet_itens)

        const req = await fetch(`https://petrakka.com:7231/api/Animal/animal/${itens}`)
        const res_pet = await req.json()
        name_pet_item.textContent = "Nome Pet: "+res_pet.nome
    }
}

btn_save.addEventListener("click",async()=>{
    if(!selectedPetId){
        showMessage("Selecione um pet primeiro","red")
        return
    }
    if(date_atend.value===""){
        showMessage("Selecione a data","red")
        return
    }

    let data_formatada = date_atend.value+"T09:00:00.000"

    const obj = {
        animalId:selectedPetId,
        _data:data_formatada,
        Sintomas:value_diag.value,
        Tratamento:type_presc.value,
        Observacoes:type_atend.value,
        VeterinarioId:""
    }

    try{
        const req = await fetch("https://petrakka.com:7231/api/RelatorioClinico/register",{
            method:"POST",
            headers:{"Content-Type":"application/json"},
            body:JSON.stringify(obj)
        })
        const res = await req.json()
        showMessage("Consulta cadastrada com sucesso","green")
        resetForm()
    }catch(error){}
})

const btn_vet = document.getElementById("btn_vet")
btn_vet.addEventListener("click",async e=>{
    const searchHistVet = document.getElementById("searchHistVet").value
    search_results.innerHTML = ""

    try{
        const req_hist_vet = await fetch(`https://petrakka.com:7231/api/Agendamento/veterinario/${searchHistVet}`)
        const res_hist_vet = await req_hist_vet.json()

        if(req_hist_vet.status === 404){
            showMessage(res_hist_vet.message,"red")
            return
        }

        for(let i=0;i<res_hist_vet.length;i++){
            if(res_hist_vet[i].id){
                const req_info_pet = await fetch(`https://petrakka.com:7231/api/Animal/animal/${res_hist_vet[i].animalId}`)
                const res_info_pet = await req_info_pet.json()
                let name_pet = res_info_pet.id ? res_info_pet.nome : ""

                const req_info_vet = await fetch(`https://petrakka.com:7231/api/MedicoVeterinario/MedicoVeterinario/${searchHistVet}`)
                const res_info_vet = await req_info_vet.json()
                let name_vet = res_info_vet.id ? res_info_vet.nome : ""

                const div_card_hist_consul = document.createElement("div")
                div_card_hist_consul.classList.add("card_hist_consulta")

                const h3_name_pet = document.createElement("h3")
                h3_name_pet.innerText = "Consulta — "+name_pet

                const p_data = document.createElement("p")
                const span_data = document.createElement("span")
                const span_vet = document.createElement("span")

                const iso = res_hist_vet[i].dataConsulta
                const dataFormatada = iso.slice(8,10) + "/" + iso.slice(5,7) + "/" + iso.slice(0,4) + " " + iso.slice(11,16)

                span_data.innerText = "Data: "+dataFormatada+"   "
                span_vet.innerText = "Veterinário: "+name_vet

                const div_stats = document.createElement("div")
                if(res_hist_vet[i].status == 0){
                    div_stats.classList.add("status_tag","status-pending")
                    div_stats.innerText = "Pendente"
                }else if(res_hist_vet[i].status == 1){
                    div_stats.classList.add("status_tag","status-progress")
                    div_stats.innerText = "Em andamento"
                }else{
                    div_stats.classList.add("status_tag","status-done")
                    div_stats.innerText = "Concluída"
                }

                p_data.appendChild(span_data)
                p_data.appendChild(span_vet)
                div_card_hist_consul.appendChild(h3_name_pet)
                div_card_hist_consul.appendChild(p_data)
                div_card_hist_consul.appendChild(div_stats)

                search_results.appendChild(div_card_hist_consul)
            }
        }
    }catch(error){}
})

const btn_tutor = document.getElementById("btn_tutor")
btn_tutor.addEventListener("click",async e=>{
    const searchHistTutor = document.getElementById("searchHistTutor").value
    search_results_t.innerHTML = ""

    try{
        const req_hist_tutor = await fetch(`https://petrakka.com:7231/api/Agendamento/cliente/${searchHistTutor}`)
        const res_hist_tutor = await req_hist_tutor.json()

        for(let i=0;i<res_hist_tutor.length;i++){
            if(res_hist_tutor[i].id){
                const req_info_pet = await fetch(`https://petrakka.com:7231/api/Animal/animal/${res_hist_tutor[i].animalId}`)
                const res_info_pet = await req_info_pet.json()
                let name_pet = res_info_pet.id ? res_info_pet.nome : ""

                const req_info_tutor = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${searchHistTutor}`)
                const res_info_tutor = await req_info_tutor.json()
                let name_tutor = res_info_tutor.id ? res_info_tutor.firstName : ""

                const div_card_hist_consul = document.createElement("div")
                div_card_hist_consul.classList.add("card_hist_consulta")

                const h3_name_pet = document.createElement("h3")
                h3_name_pet.innerText = "Consulta — "+name_pet

                const p_data = document.createElement("p")
                const span_data = document.createElement("span")
                const span_tutor = document.createElement("span")

                const iso = res_hist_tutor[i].dataConsulta
                const dataFormatada = iso.slice(8,10)+"/"+iso.slice(5,7)+"/"+iso.slice(0,4)+" "+iso.slice(11,16)

                span_data.innerText = dataFormatada
                span_tutor.innerText = "  Tutor: "+name_tutor

                const div_stats = document.createElement("div")
                if(res_hist_tutor[i].status == 0){
                    div_stats.classList.add("status_tag","status-pending")
                    div_stats.innerText = "Pendente"
                }else if(res_hist_tutor[i].status == 1){
                    div_stats.classList.add("status_tag","status-progress")
                    div_stats.innerText = "Em andamento"
                }else{
                    div_stats.classList.add("status_tag","status-done")
                    div_stats.innerText = "Concluída"
                }

                p_data.appendChild(span_data)
                p_data.appendChild(span_tutor)
                div_card_hist_consul.appendChild(h3_name_pet)
                div_card_hist_consul.appendChild(p_data)
                div_card_hist_consul.appendChild(div_stats)

                search_results_t.appendChild(div_card_hist_consul)
            }
        }
    }catch(error){}
})

const modal = document.getElementById('modalHist')
const btn = document.getElementById('btnVerMais')
const close = document.getElementById('closeModal')
const modalHistTutor = document.getElementById('modalHistTutor')
const closeModalTutor = document.getElementById('closeModalTutor')
const btn_tutor_mais = document.getElementById('btnVerMaisTutor')

btn.onclick = () => modal.style.display = 'flex'
close.onclick = () => modal.style.display = 'none'
btn_tutor_mais.onclick = () => modalHistTutor.style.display = 'flex'
closeModalTutor.onclick = () => modalHistTutor.style.display = 'none'
window.onclick = e => {
    if(e.target == modalHistTutor){
        modalHistTutor.style.display = 'none'
    }else if(e.target == modal){
        modal.style.display = "none"
    }
}



async function teste(){
    try {
        const req = await fetch(`https://petrakka.com:7231/api/RelatorioClinico/Relatorios?animalId=${"aa8b5f25-3d16-4b82-9f58-7edf1a3af676"}`)
        const res = await req.json()
        console.log(res)
    } catch (error) {
        
    }
}

teste()
