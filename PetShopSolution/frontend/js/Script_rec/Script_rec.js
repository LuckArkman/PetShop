const rg_tutor = document.getElementById("rg_tutor").value
document.addEventListener("DOMContentLoaded", async () => {
  const appointmentList = document.querySelector(".appointment-list")
  const appointmentCount = document.getElementById("appointment-count")
  const noAppointmentsMsg = document.getElementById("no-appointments-message")
  try {
    const reqConsultas = await fetch("https://petrakka.com:7231/api/Agendamento/hoje", {
      method: "GET",
      headers: { "Authorization": `Bearer ${token}` }
    })

    if (!reqConsultas.ok) throw new Error("Erro ao buscar consultas de hoje")

    const consultas = await reqConsultas.json()
    console.log("Consultas de hoje:", consultas)

    appointmentList.innerHTML = ""
    
    if (!consultas || consultas.length === 0) {
      noAppointmentsMsg.style.display = "block"
      appointmentCount.textContent = "(0)"
      return
    }

    noAppointmentsMsg.style.display = "none"
    appointmentCount.textContent = `(${consultas.length})`

    /*for (const consulta of consultas) {
      const { id, animalId,clienteId,dataConsulta } = consulta

      const reqResponsavel = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${clienteId}`, {
        method: "GET",
        headers: { "Authorization": `Bearer ${token}` }
      })
      const responsavel = await reqResponsavel.json()

      const reqAnimal = await fetch(`https://petrakka.com:7231/api/Animal/animal?animal=${animalId}`, {
        method: "GET",
        headers: { "Authorization": `Bearer ${token}` }
      })
      const animal = await reqAnimal.json()

      const data = new Date(dataConsulta)
      const hora = data.toISOString().substring(11, 16)

      const card = document.createElement("div")
      card.classList.add("appointment-card")

      const timeDiv = document.createElement("div")
      timeDiv.classList.add("time")
      timeDiv.textContent = hora

      const detailsDiv = document.createElement("div")
      detailsDiv.classList.add("details")
      detailsDiv.innerHTML = `
        <p class="pet-name">${animal.Nome || "Pet não identificado"}</p>
        <p class="owner-info">Tutor(a): ${responsavel.FirstName || "Desconhecido"} | RG: ${responsavel.RG || "Não informado"}</p>
      `

      const cancelBtn = document.createElement("button")
      cancelBtn.classList.add("cancel-btn")
      cancelBtn.textContent = "Cancelar"

      cancelBtn.addEventListener("click", async () => {
        if (confirm(`Deseja cancelar a consulta de ${animal.Nome}?`)) {
          try {
            const delReq = await fetch(`https://petrakka.com:7231/api/Agendamento/${id}`, {
              method: "DELETE",
              headers: { "Authorization": `Bearer ${token}` }
            })

            if (delReq.ok) {
              showMessage("Consulta cancelada com sucesso", "green")
              card.remove()
            } else {
              showMessage("Erro ao cancelar consulta", "red")
            }
          } catch (err) {
            console.log(err)
            showMessage("Erro interno", "red")
          }
        }
      })

      card.appendChild(timeDiv)
      card.appendChild(detailsDiv)
      card.appendChild(cancelBtn)
      appointmentList.appendChild(card)
    }*/
  } catch (error) {
    showMessage("Sem consultas ao momento", "red")
  }
})



/*Function message*/
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
let calendar;
const btn_perfil = document.getElementById("btn_perfil")
btn_perfil.addEventListener("click",()=>{
    const card_perfil_config = document.getElementById("card_perfil_config")
    card_perfil_config.classList.toggle("show")
})

/*const btn_delete = document.getElementById("btn_delete")
btn_delete.addEventListener("click",(e)=>{
  const modal_delete = document.getElementById("modal-delete")
  modal_delete.classList.toggle("show_delete")
  const date = new Date()
  const day_atual = date.getDate()
  const btn_confirm_delete = document.getElementById("btn_confirm_delete")
  btn_confirm_delete.addEventListener("click",()=>{
    const data_delete = document.getElementById("data_delete").value
    const day_data_delete = data_delete.slice(8)
    if(day_data_delete<day_atual){
      showMessage("Data invalida","red")
    }

  })
})*/

const close_delete = document.getElementById("close-delete")
close_delete.addEventListener("click", () => {
  document.getElementById("modal-delete").classList.remove("show_delete")
})

window.addEventListener("click", (e) => {
  const modal = document.getElementById("modal-delete")
  if (e.target === modal) {
    modal.classList.remove("show_delete")
  }
})

const btn_data = document.getElementById("btn_data")
btn_data.addEventListener("click",(e)=>{
  const data_container = document.getElementById("data_container")
  data_container.classList.toggle("show_data")
})

//Token user
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
const payload = getPayloadFromToken(token)
const userId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
console.log(userId)
const btn_search = document.getElementById("btn_search")
btn_search.addEventListener("click",async(e)=>{
    const value_rg = document.getElementById("value_rg").value
    if(value_rg ===""){
      //Fazer msg error
      showMessage("Preencha corretamente","red")
      return
    }
    try {
      const req = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${value_rg}`,{
        method:"GET"
      })
      const res = await req.json()
      console.log(res)
      tutor_pet(res)
    } catch (error) {
      console.log(error)
    }
})


async function tutor_pet(res){
  let horario_click;
  let data_formatada_back;
  const resultado_tutor_container = document.getElementById("resultado_tutor_container") 
  resultado_tutor_container.style.display = "block"
  const pet_lista = document.getElementById("pet_lista")
  const spantutor_name = document.getElementById("spantutor_name")
  spantutor_name.textContent = res.FirstName
  const tutor_name_pet = document.getElementById("tutor_name_pet")
  tutor_name_pet.textContent = res.FirstName
  for (const itens of res.animais) {
    const div_pet_itens = document.createElement("div")
    div_pet_itens.classList.add("pet-item")
    div_pet_itens.id = itens
    div_pet_itens.addEventListener("click", () => {
      document.querySelectorAll(".pet-item")
        .forEach(c => c.classList.remove("click_item"))
      div_pet_itens.classList.add("click_item")
      const calendarEl = document.getElementById('calendar-inner')
      const calendarContainer = document.getElementById('calendar')
      const btnAgendar = document.getElementById('btn_agendar_consulta')
      const closeBtn = document.querySelector('.close-calendar')
      if (calendar) calendar.destroy()
       calendar = new FullCalendar.Calendar(calendarEl, {
        locale: 'pt-br',
        initialView: 'dayGridMonth',
        height: 500,
        headerToolbar: {
          start: '',
          center: 'title',
          end: 'prev,next'
        },
        dateClick: async function(info) {
          const horarios = document.querySelectorAll(".horario")
          const modalHorarios = document.getElementById("modal-horarios")
          modalHorarios.style.display = "block"
          horarios.forEach((horario)=>{
              horario.onclick = (e)=>{
                horarios.forEach(h => h.classList.remove("selected"))
                e.currentTarget.classList.add("selected")
                horario_click = e.currentTarget.innerHTML
                data_formatada_back = info.dateStr+"T"+horario_click+":00.000Z"
              }
          })
          try {
            const req = await fetch(`https://petrakka.com:7231/api/Agendamento/disponibilidades/${info.dateStr}`,{method: "GET"})            
            const resHorarios  = await req.json()
            horarios.forEach((horario)=>{
              horario.style.display = "block"
              horario.classList.remove("selected")
              if(!resHorarios.includes(horario.innerHTML)){
                horario.style.display = "none"
                horario.classList.remove("selected")
              }
            }) 
            const body_indisponivel = {
              Data:info.dateStr,
              Motivo:"Todos foram agendados"
            }
            console.log(info.dateStr+"T"+"00"+":00.000Z")
            if(resHorarios.length === 0){
              const reqIndisponivel = await fetch(`https://petrakka.com:7231/api/Agendamento/indisponiveis`,{
                method:"POST",
                headers:{
                 "Content-Type": "application/json"
                },
                body:JSON.stringify(body_indisponivel)
              })
              const resIndisponivel = await reqIndisponivel.json()
              showMessage("Dia indisponivel","red")
              modalHorarios.style.display = "none"
              horarios.forEach((h)=>{h.classList.remove("selected")})
            }                         
          } catch (error) {
            
          }
          /*Fim rota*/
          const closeHorarios = document.querySelector(".close-horarios")
          modalHorarios.classList.add("show")
          closeHorarios.onclick = () => modalHorarios.classList.remove("show")
          const btn_agendar_horario = document.getElementById("btn_agendar_horario")
          btn_agendar_horario.onclick = async (e) => {
            const cmrv_vet = document.getElementById("cmrv_vet").value
            if(!cmrv_vet || !rg_tutor){
              showMessage("Preencha todos os dados","red")
            }
            console.log("Data formatada:"+data_formatada_back)
            const body_consulta = {
              id:"",
              animalId:div_pet_itens.id,
              //clienteId:res.Id,
              //veterinarioId:cmrv_vet,
              rg:rg_tutor,
              crmv:cmrv_vet,
              dataConsulta:data_formatada_back,
              status:0 
            }
            try {
            const req = await fetch("https://petrakka.com:7231/api/Agendamento",{
              method:"Post",
              headers:
              {
              "Content-Type":"application/json"
              },
              body:JSON.stringify(body_consulta)
            })
            const res = await req.json()
            console.log(res.id)
            if(res.animalId){
              modalHorarios.classList.remove("show")
              calendarContainer.classList.remove('show-calendar')
              showMessage(`Dia ${info.dateStr} agendado com sucesso`,"green")
            }else{
              showMessage("Erro ao agendar tente novamente","red")
            }
          } catch (error) {
            showMessage("Erro interno","red")
          }
          }
        },
        datesSet: async function(info) {
          try {
            const req = await fetch("https://petrakka.com:7231/api/Agendamento/indisponibilidades-detalhadas",{method:"GET"})
            const res = await req.json()
            calendar.removeAllEvents()
            res.forEach(dia => {
              calendar.addEvent({
                start: dia.Data,
                display: 'background',
                backgroundColor: '#ff4d4d',
                overlap: false
              })
            })      
        } catch (error) {
            showMessage("Erro interno","red")
        }
      }
      })
    
      closeBtn.addEventListener('click', () => {
        calendarContainer.classList.remove('show-calendar')
      })
    
      calendarContainer.addEventListener('click', (e) => {
        if (e.target === calendarContainer) {
          calendarContainer.classList.remove('show-calendar')
        }
      })
      calendarContainer.classList.add("show-calendar")
      calendar.render()
    })
    resultado_tutor_container.appendChild(div_pet_itens)
    const pet_icone = document.createElement("span")
    pet_icone.classList.add("pet-icone")
    pet_icone.textContent = "🐾"
    const name_pet = document.createElement("p")
    name_pet.classList.add("pet-nome-placeholder")
    const btn_pet_ver_mais = document.createElement("button")
    btn_pet_ver_mais.classList.add("btn_info_user")
    btn_pet_ver_mais.textContent = "Ver Mais"
    btn_pet_ver_mais.addEventListener("click", async () => {
      try {
        const req = await fetch(`https://petrakka.com:7231/api/Animal/animal/${itens}`)
        const pet = await req.json()
        document.getElementById("modal_pet_nome").textContent = pet.nome || "Não informado"
        document.getElementById("modal_pet_especie").textContent = pet.especie || "Não informado"
        document.getElementById("modal_pet_raca").textContent = pet.raca || "Não informado"
        document.getElementById("modal_pet_sexo").textContent = pet.sexo || "Não informado"
        document.getElementById("modal_pet_idade").textContent = pet.idade ?? "Não informado"
        document.getElementById("modal_pet_peso").textContent = pet.peso ?? "Não informado"
        document.getElementById("modal_pet_porte").textContent = pet.porte || "Não informado"
        document.getElementById("modal_pet_castrado").textContent = pet.castrado ? "Sim" : "Não"
        const modalPet = document.getElementById("modal-pet")
        modalPet.style.display = "flex"
        const closePet = document.querySelector(".close-pet")
        closePet.onclick = () => modalPet.style.display = "none"
        window.onclick = (event) => {
          if (event.target === modalPet) {
            modalPet.style.display = "none"
          }
        }
      } catch (error) {
        console.log(error)
      }
    })
    div_pet_itens.appendChild(pet_icone)
    div_pet_itens.appendChild(name_pet)
    div_pet_itens.appendChild(btn_pet_ver_mais)
    pet_lista.appendChild(div_pet_itens)
    try {
      const req = await fetch(`https://petrakka.com:7231/api/Animal/animal/${itens}`)
      const res = await req.json()
      name_pet.textContent = `Nome Pet:${res.nome}`
    } catch (error) {
      console.log(error)
    }

    /*Logica botões ver mais*/
    const modal = document.getElementById("modal-user")
    const closeBtn = document.querySelector(".close")
    document.addEventListener("click", function(e) {
      if (e.target && e.target.id === "btn_info_user") {
        document.getElementById("modal_nome").textContent = `${res.FirstName} ${res.LastName || ""}`
        document.getElementById("modal_email").textContent = res.Email || "Não informado"
        document.getElementById("modal_rg").textContent = res.RG || "Não informado"
        document.getElementById("modal_cpf").textContent = res.CPF || "Não informado"
        document.getElementById("modal_telefone").textContent = res.PhoneNumber || "Não informado"
        document.getElementById("modal_city").textContent = res.City || "Não informado"
        document.getElementById("modal_state").textContent = res.State || "Não informado"
        modal.style.display = "flex"
      }
    })
    closeBtn.onclick = function() {
      modal.style.display = "none"
    }

    window.onclick = function(event) {
      if (event.target === modal) {
        modal.style.display = "none"
      }
    }

  } 
}