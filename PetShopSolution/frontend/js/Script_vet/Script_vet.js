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
document.addEventListener("DOMContentLoaded",async(e)=>{
    const body_consul_day = document.getElementById("body_consul_day")
    try {
        const req = await fetch("https://petrakka.com:7231/api/Agendamento/hoje",{method:"GET"})
        const res = await req.json()
        for(let i=0;i<res.length;i++){
            if(res[i].id){
                const req_pet = await fetch(`https://petrakka.com:7231/api/Animal/animal?animal=${res[i].animalId}`,{method:"GET"})
                const res_pet = await req_pet.json()
                console.log(res_pet)
                if(res_pet.id){
                    const req_tutor = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${res_pet.responsaveis[0]}`,{method:"GET"})
                    const res_tutor = await req_tutor.json()
                    let horario_form = res[i].dataConsulta.slice(11,16)
                    //horario,nome pet, nome tutor,atendimento
                    let tr_body_itens = document.createElement("tr")
                    let td_horario =  document.createElement("td")
                    let td_nome_pet =  document.createElement("td")
                    let td_nome_tutor =  document.createElement("td")
                    let stats = document.createElement("span")
                    td_horario.innerText = horario_form
                    td_nome_pet.innerText = res_pet.Nome
                    td_nome_tutor.innerText = res_tutor.FirstName
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
                    body_consul_day.appendChild(tr_body_itens)
                    tr_body_itens.addEventListener("click",async(e)=>{
                        name_pet.value = td_nome_pet.innerText
                        name_tutor.value = td_nome_tutor.innerText
                        try {
                            await fetch(`https://petrakka.com:7231/api/Agendamento/${res[i].id}/status?status=1`, {
                                method: "PATCH",
                                headers: { "Content-Type": "application/json" }
                            })
                            stats.classList.remove("status-pending", "status-done")
                            stats.classList.add("status-progress")
                            stats.innerText = "Em andamento"
                        } catch (error) {
                            console.error("Erro ao atualizar status para 'Em andamento':", error)
                        }
                        console.log(res)
                        btn_save.addEventListener("click",async(e)=>{
                            let data_formatada = date_atend.value+"T"+td_horario.innerText+":00.000"
                            console.log(data_formatada)
                            const obj_dados_rel = {
                                animalId:res_pet.id,
                                _data:data_formatada,
                                Sintomas:value_diag.value,
                                Tratamento:type_presc.value,
                                Observacoes:type_atend.value,
                                VeterinarioId:""
                            }
                            console.log(obj_dados_rel)
                            try {
                                const req_rel = await fetch("https://petrakka.com:7231/api/RelatorioClinico/register",{method:"POST",headers:{"Content-Type":"application/json"},body:JSON.stringify(obj_dados_rel)})
                                const res_rel = await req_rel.json()
                                console.log(res_rel)
                                if(res_rel.Id){
                                    await fetch(`https://petrakka.com:7231/api/Agendamento/${res[i].id}/status?status=2`, {
                                        method: "PATCH",
                                        headers: {"Content-Type": "application/json"}
                                    })
                                    stats.classList.remove("status-pending","status-progress")
                                    stats.classList.add("status-done")
                                    stats.innerText="Concluída"
                                    showMessage("consulta realizada com sucesso","green")
                                }
                            } catch (error) {
                                console.log(error)
                            }
                        })
                    })
                }
            }
        }
    } catch (error) {
        console.log(error)
    }
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

  //Logica buscar histórico tutor/vet
  const btn_vet = document.getElementById("btn_vet")
  btn_vet.addEventListener("click",async(e)=>{
    const searchHistVet = document.getElementById("searchHistVet").value
    console.log(searchHistVet)
    //Chamar rota
    try {
        const req_hist_vet = await fetch(`https://petrakka.com:7231/api/Agendamento/veterinario/${searchHistVet}`,{method:"GET"})
        const res_hist_vet = await req_hist_vet.json()
        console.log(res_hist_vet)
        for(let i=0;i<res_hist_vet.length;i++){
            if(res_hist_vet[i].id){
                console.log("passou")
                //pegar info pet
                const req_info_pet = await fetch(`https://petrakka.com:7231/api/Animal/animal?animal=${res_hist_vet[i].animalId}`,{method:"GET"})
                const res_info_pet = await req_info_pet.json()
                let name_pet;
                if(res_info_pet.id){
                    name_pet = res_info_pet.Nome
                }
                //12345-SP 
                const req_info_vet = await fetch(`https://petrakka.com:7231/api/MedicoVeterinario/MedicoVeterinario?crmv=${searchHistVet}`,{method:"GET"})
                const res_info_vet = await req_info_vet.json()
                console.log("Resposta vet:", res_info_vet)
                console.log("Resposta vet:", req_info_vet)
                let name_vet;
                if(res_info_vet.Id){
                    name_vet = res_info_vet.Nome
                }
                const div_card_hist_consul = document.createElement("div")
                div_card_hist_consul.classList.add("card_hist_consulta")
                const h3_name_pet = document.createElement("h3")
                const p_data_consul_and_name_vet = document.createElement("p")
                const span_data_consul = document.createElement("span")
                const span_name_vet = document.createElement("span")
                const div_stats = document.createElement("div")
                h3_name_pet.innerText = "Consulta — "+name_pet
                const iso = res_hist_vet[i].dataConsulta
                const dataFormatada = iso.slice(8,10) + "/" + iso.slice(5,7) + "/" + iso.slice(0,4) + " " + iso.slice(11,16)
                //"Veterinário: "+name_pet
                span_data_consul.innerText = "Data: "+dataFormatada+"   "
                span_name_vet.innerText = "Veterinário: "+name_vet
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
                p_data_consul_and_name_vet.appendChild(span_data_consul)
                p_data_consul_and_name_vet.appendChild(span_name_vet)
                div_card_hist_consul.appendChild(h3_name_pet)
                div_card_hist_consul.appendChild(p_data_consul_and_name_vet)
                div_card_hist_consul.appendChild(div_stats)
                search_results.appendChild(div_card_hist_consul)
            }
        }
    } catch (error) {
        console.log(error)
    }
  })


  const btn_tutor = document.getElementById("btn_tutor")
  btn_tutor.addEventListener("click",async(e)=>{
    const searchHistTutor = document.getElementById("searchHistTutor").value
    console.log(searchHistTutor)
    try {
        const req_hist_tutor = await fetch(`https://petrakka.com:7231/api/Agendamento/cliente/${searchHistTutor}`,{method:"GET"})
        const res_hist_tutor = await req_hist_tutor.json()
        console.log(res_hist_tutor)
        for(let i=0;i<res_hist_tutor.length;i++){
            if(res_hist_tutor[i].id){
                console.log("passou")
                const req_info_pet = await fetch(`https://petrakka.com:7231/api/Animal/animal?animal=${res_hist_tutor[i].animalId}`,{method:"GET"})
                const res_info_pet = await req_info_pet.json()
                let name_pet
                if(res_info_pet.id){
                    name_pet = res_info_pet.Nome
                }
                const req_info_tutor = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel?id=${searchHistTutor}`,{method:"GET"})
                const res_info_tutor = await req_info_tutor.json()
                console.log("Resposta tutor",res_info_tutor)
                let name_tutor
                if(res_info_tutor.Id){
                    name_tutor = res_info_tutor.FirstName
                }
                const div_card_hist_consul = document.createElement("div")
                div_card_hist_consul.classList.add("card_hist_consulta")
                const h3_name_pet = document.createElement("h3")
                const p_data_consul_and_name_vet = document.createElement("p")
                const span_data_consul = document.createElement("span")
                const span_name_tutor = document.createElement("span")
                const div_stats = document.createElement("div")
                h3_name_pet.innerText = "Consulta — "+name_pet
                const iso = res_hist_tutor[i].dataConsulta
                const dataFormatada = iso.slice(8,10)+"/"+iso.slice(5,7)+"/"+iso.slice(0,4)+" "+iso.slice(11,16)
                span_data_consul.innerText = dataFormatada
                span_name_tutor.innerText = "  Tutor: "+name_tutor
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
                p_data_consul_and_name_vet.appendChild(span_data_consul)
                p_data_consul_and_name_vet.appendChild(span_name_tutor)
                div_card_hist_consul.appendChild(h3_name_pet)
                div_card_hist_consul.appendChild(p_data_consul_and_name_vet)
                div_card_hist_consul.appendChild(div_stats)
                search_results_t.appendChild(div_card_hist_consul)
            }
        }
    } catch (error) {
        console.log(error)
    }
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