document.addEventListener("DOMContentLoaded",async(e)=>{
    const total_consul = document.getElementById("total_consul")
    const fatu_pagament = document.getElementById("fatu_pagament")
    const consul_pendent = document.getElementById("consul_pendent")
    const total_users = document.getElementById("total_users")
    const total_vet = document.getElementById("total_vet")
    const total_rec = document.getElementById("total_rec")
    let pagas = 0
    let pendentes = 0
    //rota para controlar consultas
    try {
        const req_recents_consul = await fetch("https://petrakka.com:7231/api/Agendamento/recentes")
        const res_recents_consul = await req_recents_consul.json()
        console.log(res_recents_consul)
        total_consul.innerText = res_recents_consul.length
        for(let i = 0; i<res_recents_consul.length;i++){
            console.log(res_recents_consul[i].status)
            if(res_recents_consul[i].status == 2){
                pagas+=1
                fatu_pagament.innerText = pagas
            }
            if(res_recents_consul[i].status == 0){
                pendentes+=1
                consul_pendent.innerText = pendentes
            }
        }
    } catch (error) {
        console.log(error)
    }
    //rota para definir resumo do sistema
    try {
        const req_resum_users = await fetch("https://petrakka.com:7231/api/Responsavel/Responsaveis")
        const res_resum_users = await req_resum_users.json()
        total_users.innerText = res_resum_users.length
    } catch (error) {
        console.log(error)
    }

    try {
        const req_resum_users = await fetch("https://petrakka.com:7231/api/Atendente/Atendentes")
        const res_resum_users = await req_resum_users.json()
        total_rec.innerText = res_resum_users.length
    } catch (error) {
        console.log(error)
    }

    try {
        const req_resum_users = await fetch("https://petrakka.com:7231/api/MedicoVeterinario/MedicoVeterinarios")
        const res_resum_users = await req_resum_users.json()
        total_vet.innerText = res_resum_users.length
    } catch (error) {
        console.log(error)
    }

    //rota ultimos agendamentos
    try {
        const content_user = document.getElementById("content_user")
        const req_users = await fetch("https://petrakka.com:7231/api/Agendamento/hoje")
        const p_sem_pag_consul = document.getElementById("p_sem_pag_consul")
        p_sem_pag_consul.style.display = "none"
        if(req_users.status == 204){
            content_user.style.display = "none"
            p_sem_pag_consul.style.display = "flex"
            return
        }
        const res_users = await req_users.json()
        if(res_users[0].id){
            for(let i = 0; i<res_users.length;i++){
                const req_user = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${res_users[i].rg}`)
                const res_user = await req_user.json()
                if(res_user.id){
                    console.log(res_user)    
                    try {
                        const req_pet = await fetch(`https://petrakka.com:7231/api/Animal/animal/${res_users[i].animalId}`)
                        const res_pet = await req_pet.json()
                        console.log(res_pet)
                        const tr = document.createElement("tr")
                        const td_paciente = document.createElement("td")
                        const td_tutor = document.createElement("td")
                        const td_status = document.createElement("td")
                        td_paciente.textContent = res_pet.nome
                        td_tutor.textContent = res_user.firstName
                        td_status.textContent = res_users[i].status
                        tr.appendChild(td_paciente)
                        tr.appendChild(td_tutor)
                        tr.appendChild(td_status)
                        content_user.appendChild(tr)
                    } catch (error) {
                        console.log(error)
                    }
                }
            }
        }
    } catch (error) {
        console.log(error)
    }

    //rotas ultimos pagamentos
    const data_atual = new Date()
    const dataPagaments = data_atual.toISOString().split("T")[0] + "T00:00:00"
    try {
        const req_recents_pagaments = await fetch(`https:petrakka.com:7231/api/Caixa/GetPagamentosCompletosDoDiaAsync/${dataPagaments}`)
        const res_recents_pagaments = await req_recents_pagaments.json()
        const table_content = document.getElementById("table_content")
        const p_sem_pag = document.getElementById("p_sem_pag")
        p_sem_pag.style.display = "none"
        console.log(res_recents_pagaments)
        if(res_recents_pagaments.length == 0){
            table_content.style.display = "none"
            p_sem_pag.style.display = "flex"
        }
    } catch (error) {
        console.log(error)
    }
})