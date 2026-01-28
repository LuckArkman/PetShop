document.addEventListener("DOMContentLoaded", async () => {
  const btn_out_page = document.getElementById("btn_out_page")
  btn_out_page.addEventListener("click", (e) => {
    window.location.href = "../../pages/pages_cad/Cad_user.html"
  })
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
    console.log(consultas)
    appointmentList.innerHTML = ""

    if (!consultas || consultas.length === 0) {
      noAppointmentsMsg.style.display = "block"
      appointmentCount.textContent = "(0)"
      return
    }

    noAppointmentsMsg.style.display = "none"
    appointmentCount.textContent = `(${consultas.length})`

    for (const consulta of consultas) {
      const { id, animalId, dataConsulta, rg } = consulta
      let status_name = "Pago (Dinheiro)"
      let style_color = "green"
      let status_consul
      try {
        const req_status_consul = await fetch(`https://petrakka.com:7231/api/Caixa/Checkout/${id}`,{
          method:"GET"
        })
      if (req_status_consul.status !== 204 && req_status_consul.ok) {
        const res_status_consul = await req_status_consul.json()
        console.log(res_status_consul)
        status_consul = res_status_consul.id
      if (res_status_consul.status == "canceled") {
        status_name = "Não foi pago"
        style_color = "red"
      } else if (res_status_consul.status == "Pending") {
        status_name = "Pendente"
        style_color = "orange"
      } else if (res_status_consul.status   == "Complete") {
        status_name = "Pago"
        style_color = "green"
      }
      }
      const reqResponsavel = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${rg.replace(/\D/g,"")}`, {
        method: "GET",
        headers: { "Authorization": `Bearer ${token}` }
      })
      const responsavel = await reqResponsavel.json()
      console.log(responsavel)
      const reqAnimal = await fetch(`https://petrakka.com:7231/api/Animal/animal/${animalId}`, {
        method: "GET",
        headers: { "Authorization": `Bearer ${token}` }
      })
      const animal = await reqAnimal.json()
      console.log(animal)
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
      <p class="pet-name">${animal.nome || "Pet não identificado"}</p>
      <p class="owner-info">Tutor(a): ${responsavel.firstName || "Desconhecido"} | RG: ${responsavel.rg || "Não informado"} | <span  style="color:${style_color};">status:${status_name}</span> </p>`
      const cancelBtn = document.createElement("button")
      cancelBtn.classList.add("cancel-btn")
      cancelBtn.textContent = "Cancelar"

      const btn_confirm_pagament = document.createElement("button")
      btn_confirm_pagament.classList.add("confirm-btn")
      btn_confirm_pagament.textContent = "Confirmar pagamento"
      if(status_name === "Pago (Dinheiro)"){
        btn_confirm_pagament.style.display = "none"
      }
      btn_confirm_pagament.addEventListener("click", async (e) => {
        if ( status_name == "Pago") {
          showMessage("Pagamento já confirmado", "red")
          return
        }
        try {
          const req_status_pagament = await fetch(`https://petrakka.com:7231/api/Caixa/UpdateStatusAsync?id=${status_consul}&status=2`, {
            method: "POST"
          })
          const res_status_pagament = await req_status_pagament.json()
          console.log(res_status_pagament)
          if (res_status_pagament.id) {
            showMessage("Pagamento Confirmado com sucesso", "green")
            setTimeout(() => location.reload(), 800)
          } else {
            showMessage("Erro ao confirmar pagamento", "red")
          }
        } catch (error) {
          showMessage("Erro interno", "red")
          console.log(error)
        }
      })

      cancelBtn.addEventListener("click", async () => {
        if (confirm(`Deseja cancelar a consulta de ${animal.nome}?`)) {
          try {
            const delReq = await fetch(`https://petrakka.com:7231/api/Agendamento/${id}`, {
              method: "DELETE",
              headers: { "Authorization": `Bearer ${token}` }
            })

            if (delReq.ok) {
              showMessage("Consulta cancelada com sucesso", "green")
              setTimeout(() => location.reload(), 800)
              card.remove()
            } else {
              showMessage("Erro ao cancelar consulta", "red")
            }
          } catch (err) {

            showMessage("Erro interno", "red")
          }
        }
      })

      card.appendChild(timeDiv)
      card.appendChild(detailsDiv)
      card.appendChild(cancelBtn)
      card.appendChild(btn_confirm_pagament)
      appointmentList.appendChild(card)
      } catch (error) {
        console.log(error)
      }
    }
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
btn_perfil.addEventListener("click", () => {
  const card_perfil_config = document.getElementById("card_perfil_config")
  card_perfil_config.classList.toggle("show")
})

const btn_delete = document.getElementById("btn_delete")
btn_delete.onclick = () => {
  const modal_delete = document.getElementById("modal-delete")
  modal_delete.classList.toggle("show_delete")
  const date = new Date()
  const day_atual = date.getDate()
  const btn_confirm_delete = document.getElementById("btn_confirm_delete")
  btn_confirm_delete.onclick = async () => {
    const data_delete = document.getElementById("data_delete").value
    const horario_delete = document.getElementById("horario_delete").value
    const day_data_delete = data_delete.slice(8)
    if (day_data_delete < day_atual) {
      showMessage("Data invalida", "red")
    }
    try {
      const req_delete_date = await fetch(`https://petrakka.com:7231/api/Agendamento/por-data/${data_delete}/${horario_delete}`, {
        method: "DELETE"
      })
      const res_delete_date = await req_delete_date.json()


      if (req_delete_date.status === 200) {
        showMessage(res_delete_date.message, "green")
      } else {
        showMessage(res_delete_date.message, "red")
      }
      //api/Agendamento/por-data/{data}/{hora}
    } catch (error) {
      showMessage("Erro interno")

    }
  }
}

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

/*const btn_data = document.getElementById("btn_data")
btn_data.addEventListener("click",(e)=>{
  const data_container = document.getElementById("data_container")
  data_container.classList.toggle("show_data")
})*/

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
function validarRG(rg) {
  if (!rg) return false
  rg = rg.replace(/\D/g, "")
  if (rg.length < 7 || rg.length > 9) return false
  if (/^(\d)\1+$/.test(rg)) return false
  return true
}

function validarCPF(cpf) {
  cpf = cpf.replace(/\D/g, "")
  if (cpf.length !== 11) return false
  if (/^(\d)\1+$/.test(cpf)) return false

  let soma = 0
  let resto

  for (let i = 1; i <= 9; i++) {
    soma += parseInt(cpf.substring(i - 1, i)) * (11 - i)
  }

  resto = (soma * 10) % 11
  if (resto === 10 || resto === 11) resto = 0
  if (resto !== parseInt(cpf.substring(9, 10))) return false

  soma = 0
  for (let i = 1; i <= 10; i++) {
    soma += parseInt(cpf.substring(i - 1, i)) * (12 - i)
  }

  resto = (soma * 10) % 11
  if (resto === 10 || resto === 11) resto = 0
  if (resto !== parseInt(cpf.substring(10, 11))) return false

  return true
}

const btn_search = document.getElementById("btn_search")

btn_search.onclick = async () => {
  const value_rg = document.getElementById("value_rg").value.replace(/\D/g, "")

  if (!validarRG(value_rg)) {
    showMessage("RG inválido", "red")
    return
  }
  const req = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${value_rg}`)

  if (req.status === 204) {
    document.getElementById("modal-user-not-found").style.display = "flex"
    document.getElementById("cad_rg").value = value_rg
    return
  }

  if (!req.ok) {
    showMessage("Erro ao buscar usuário", "red")
    return
  }

  const res = await req.json()
  tutor_pet(res)
}

document.getElementById("btn_cancel_modal_user").onclick = () => {
  document.getElementById("modal-user-not-found").style.display = "none"
}

document.getElementById("btn_cadastrar_user").onclick = async () => {
  const modal_cadastro_user = document.getElementById("modal-cadastro-user")
  const close_cadastro_user = document.getElementById("close-cadastro-user")
  const confirm_cadastro_user = document.getElementById("confirm_cadastro_user")
  modal_cadastro_user.style.display = "flex"
  close_cadastro_user.addEventListener("click", (e) => {
    modal_cadastro_user.style.display = "none"
  })

  confirm_cadastro_user.addEventListener("click", async (e) => {
    const nome = document.getElementById("cad_nome").value.trim()
    const email = document.getElementById("cad_email").value.trim()
    const rg = document.getElementById("cad_rg").value.replace(/\D/g, "")
    const cpf = document.getElementById("cad_cpf").value.replace(/\D/g, "")
    const senha = document.getElementById("cad_senha").value
    const confSenha = document.getElementById("cad_conf_senha").value

    if (!validarRG(rg)) {
      showMessage("RG inválido", "red")
      return
    }
    //985.696.370-20
    if (!validarCPF(cpf)) {
      showMessage("CPF inválido", "red")
      return
    }

    if (!nome || !email || !rg || !cpf || !senha) {
      showMessage("Preencha todos os campos", "red")
      return
    }

    if (senha !== confSenha) {
      showMessage("Senhas não coincidem", "red")
      return
    }

    const req = await fetch("https://petrakka.com:7231/api/Responsavel/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        FirstName: nome,
        Email: email,
        RG: rg,
        Cpf: cpf,
        Password: senha,
        ConfirmPassword: confSenha
      })
    })

    const res = await req.json()
    console.log(res)
    /*if(){
      showMessage("Erro ao cadastrar","red")
      return
    }*/

    showMessage("Usuário cadastrado com sucesso", "green")
    setTimeout(() => {
      document.getElementById("modal-user-not-found").style.display = "none"
      modal_cadastro_user.style.display = "none"
    }, 800)
  })
}

function abrirModalCadastroPet(rgTutor) {
  const modal = document.querySelector(".modal_pet")
  modal.style.display = "flex"

  document.getElementById("fRg").value = rgTutor

  document.getElementById("cancelSimple").onclick = () => {
    modal.style.display = "none"
  }

  document.getElementById("saveSimple").onclick = async () => {
    await cadastrarPet(rgTutor)
  }
}

async function cadastrarPet(rgTutor) {
    const fRg = document.getElementById("fRg").value
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
    const value_fCastrado = fCastrado === "Sim"
    if(isNaN(fAge) || isNaN(fWeight)){
        showMessage("Dados inválidos digite números inteiro para idade e peso","red")
        return
    }
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

  try {
    const req_tutor = await fetch(`https://petrakka.com:7231/api/Responsavel/Responsavel${rgTutor}`,{
      method:"GET"
    })
    const res_tutor = await req_tutor.json()
    console.log(res_tutor)
    if(res_tutor.id){
    const req = await fetch("https://petrakka.com:7231/api/Animal/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
                nome: fName,
                especie: fSpecies,
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
                responsaveis: [res_tutor.id]
    })})

    const res = await req.json()
    console.log(res)
    if (res.id) {
      // 🔥 adiciona o pet no array de animais do tutor
const bodyUpdateTutor = {
  id: res_tutor.id,
  email: res_tutor.email,
  firstName: res_tutor.firstName,
  lastName: res_tutor.lastName,
  cpf: res_tutor.cpf,
  rg: res_tutor.rg,
  password: "admin",           // se o backend exigir
  confirmPassword: "admin",    // se o backend exigir
  address: res_tutor.address,
  city: res_tutor.city,
  state: res_tutor.state,
  zipCode: res_tutor.zipCode,
  phoneNumber: res_tutor.phoneNumber,
  animais: [
    ...(res_tutor.animais ?? []),
    res.id // 👈 ID DO PET
  ]
}

const reqUpdateTutor = await fetch(
  "https://petrakka.com:7231/api/Responsavel/update",
  {
    method: "PUT",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(bodyUpdateTutor)
  }
)

      const resUpdateTutor = await reqUpdateTutor.json()
      if(resUpdateTutor.user){
        showMessage("Pet cadastrado com sucesso 🐾", "green")
        setTimeout(() => location.reload(), 800)
      }else{
        showMessage("Erro ao cadastrar pet","red")
      }
    }
    }else{
      showMessage("Erro ao cadastrar pet","red")
    }
  } catch (error) {
    console.error(error)
    showMessage("Erro interno", "red")
  }
}

async function tutor_pet(res) {
  let horario_click;
  let data_formatada_back;
  const resultado_tutor_container = document.getElementById("resultado_tutor_container")
  resultado_tutor_container.style.display = "block"
  const pet_lista = document.getElementById("pet_lista")
  const spantutor_name = document.getElementById("spantutor_name")
  spantutor_name.textContent = res.firstName
  const tutor_name_pet = document.getElementById("tutor_name_pet")
  tutor_name_pet.textContent = res.firstName
  if (res.animais == null) {
    const modalPetNotFound = document.getElementById("modal-pet-not-found")
    modalPetNotFound.style.display = "flex"
    document.getElementById("btn_cancel_modal_pet").onclick = () => {
      modalPetNotFound.style.display = "none"
      }
    document.getElementById("btn_cadastrar_pet").onclick = () => {
      modalPetNotFound.style.display = "none"
      abrirModalCadastroPet(res.rg)
    }
    return
  }
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
        dateClick: async function (info) {
          const horarios = document.querySelectorAll(".horario")
          const modalHorarios = document.getElementById("modal-horarios")
          modalHorarios.style.display = "block"
          horarios.forEach((horario) => {
            horario.onclick = (e) => {
              horarios.forEach(h => h.classList.remove("selected"))
              e.currentTarget.classList.add("selected")
              horario_click = e.currentTarget.innerHTML
              data_formatada_back = info.dateStr + "T" + horario_click + ":00.000Z"
            }
          })
          try {
            const req = await fetch(`https://petrakka.com:7231/api/Agendamento/disponibilidades/${info.dateStr}`, { method: "GET" })
            const resHorarios = await req.json()
            horarios.forEach((horario) => {
              horario.style.display = "block"
              horario.classList.remove("selected")
              if (!resHorarios.includes(horario.innerHTML)) {
                horario.style.display = "none"
                horario.classList.remove("selected")
              }
            })
            const body_indisponivel = {
              Data: info.dateStr,
              Motivo: "Todos foram agendados"
            }

            if (resHorarios.length === 0) {
              const reqIndisponivel = await fetch(`https://petrakka.com:7231/api/Agendamento/indisponiveis`, {
                method: "POST",
                headers: {
                  "Content-Type": "application/json"
                },
                body: JSON.stringify(body_indisponivel)
              })
              const resIndisponivel = await reqIndisponivel.json()
              showMessage("Dia indisponivel", "red")
              modalHorarios.style.display = "none"
              horarios.forEach((h) => { h.classList.remove("selected") })
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
            const rg_tutor = document.getElementById("rg_tutor").value.replace(/\D/g, "")
            const pagament_din = document.getElementById("pagament_din").value
            let status_agendament;
            if (pagament_din === "pag_true") {
              status_agendament = 2
            } else {
              status_agendament = 1
            }
            console.log(status_agendament)
            if (pagament_din === "default_pagament") {
              showMessage("informe se já foi pago", "red")
              return
            }
            if (!cmrv_vet || !rg_tutor) {
              showMessage("Preencha todos os dados", "red")
            }
            const body_consulta = {
              id:"",
              animalId: div_pet_itens.id,
              rg: rg_tutor,
              crmv: cmrv_vet,
              dataConsulta: data_formatada_back,
              status: status_agendament
            }
            console.log(body_consulta)
            try {
              const req = await fetch("https://petrakka.com:7231/api/Agendamento", {
                method: "Post",
                headers:
                {
                  "Content-Type": "application/json"
                },
                body: JSON.stringify(body_consulta)
              })
              const res = await req.json()
              console.log(res)
              if (res.animalId) {
                modalHorarios.classList.remove("show")
                calendarContainer.classList.remove('show-calendar')
                const paymentModal = document.getElementById("paymentModal")
                const closeModal = document.getElementById("closeModal")
                const cancelPayment = document.getElementById("cancelPayment")
                const confirmPayment = document.getElementById("confirmPayment")
                const priceValue = document.getElementById("priceValue")
                const value_total = document.getElementById("value_total")
                const div_effect = document.getElementById("div_effect")
                if (pagament_din === "pag_true") {
                  //fazer lógica do status, e mudar para que já foi pago
                  paymentModal.style.display = "none"
                  div_effect.style.display = "none"
                } else {
                  paymentModal.style.display = "flex"
                  div_effect.style.display = "flex"
                  value_total.addEventListener("input", (e) => {
                    priceValue.innerText = `R$${e.target.value}`
                  })
                  cancelPayment.onclick = () => {
                    paymentModal.style.display = "none"
                    div_effect.style.display = "none"
                  }
                  confirmPayment.addEventListener("click", async (e) => {
                    const paymentType = document.getElementById("paymentType").value
                    const dados = {
                      consultaId: res.id,
                      valor: Number(value_total.value),
                      paymentMethod: paymentType
                    }
                    console.log(dados)
                    if (paymentType.value === "selecione") {
                      showMessage("Selecione um método de pagamento válido")
                      return
                    }
                    if (value_total.value < 1) {
                      showMessage("Valor do pagamento inválido")
                    }
                    try {
                      const req_payment = await fetch(`https://petrakka.com:7231/api/Caixa/Payment/ProcessCheckout`, {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(dados)
                      })
                      const res_payment = await req_payment.json()
                      console.log(res_payment)
                      if (res_payment.success) {
                        const pix_card = document.getElementById("pix_card")
                        const qr_code_pagament = document.getElementById("qr_code_pagament")
                        const cancel_pix = document.getElementById("cancel_pix")
                        const close_pix = document.getElementById("close_pix")
                        close_pix.addEventListener("click", (e) => {
                          pix_card.style.display = "none"
                        })
                        pix_card.style.display = "block"
                        qr_code_pagament.src = `data:image/png;base64,${res_payment.paymentData.pixQrCodeBase64}`
                        cancel_pix.addEventListener("click", async () => {
                          try {
                            const req_delete = await fetch(`https://petrakka.com:7231/api/Agendamento/${res.id}`)
                            const res_delete = await req_delete.json()
                            if (res_delete.id) {
                              pix_card.style.display = "none"
                              paymentModal.style.display = "none"
                              div_effect.style.display = "none"
                            }
                          } catch (error) {
                            console.log(error)
                          }
                        })
                        setTimeout(async (e) => {
                          try {
                            const req_verify_pagament = await fetch(`https://petrakka.com:7231/api/Caixa/GetByIdAsync/${res.id}`)
                            const res_verify_pagament = await req_verify_pagament.json()
                            console.log(res_verify_pagament)
                            if (res_verify_pagament.status == "paid") {
                              try {
                                const req_status_pagament = await fetch(`https://petrakka.com:7231/api/Caixa/UpdateStatusAsync/id?id=${res.id}/status?status=${2}`, { method: "POST" })
                                const res_status_pagament = await req_status_pagament.json()
                                if (res_status_pagament.id) {
                                  showMessage("Pagamento feito com sucesso", "green")
                                  pix_card.style.display = "none"
                                  paymentModal.style.display = "none"
                                  div_effect.style.display = "none"
                                } else {
                                  showMessage("Erro ao realizar pagamento", "green")
                                }
                              } catch (error) {
                                showMessage("Erro interno", "red")
                                console.log(error)
                              }
                            }
                          } catch (error) {
                            showMessage("Erro interno", "red")
                            console.log(error)
                          }
                        }, 3000)
                      }
                    } catch (error) {
                      showMessage("Erro interno", "red")
                      console.log(error)
                    }
                  })
                }
              } else {
                showMessage("Erro ao agendar tente novamente", "red")
              }
            } catch (error) {
              console.log(error)
              showMessage("Erro interno", "red")
            }
            /*Fluxo do webhook*/
          }
        },
        datesSet: async function (info) {
          try {
            const req = await fetch("https://petrakka.com:7231/api/Agendamento/indisponibilidades-detalhadas", { method: "GET" })
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
            showMessage("Erro interno", "red")
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

    }

    /*Logica botões ver mais*/
    const modal = document.getElementById("modal-user")
    const closeBtn = document.querySelector(".close")
    document.addEventListener("click", function (e) {
      if (e.target && e.target.id === "btn_info_user") {
        document.getElementById("modal_nome").textContent = `${res.firstName} ${res.lastName || ""}`
        document.getElementById("modal_email").textContent = res.email || "Não informado"
        document.getElementById("modal_rg").textContent = res.rg || "Não informado"
        document.getElementById("modal_cpf").textContent = res.cpf || "Não informado"
        document.getElementById("modal_telefone").textContent = res.phoneNumber || "Não informado"
        document.getElementById("modal_city").textContent = res.city || "Não informado"
        document.getElementById("modal_state").textContent = res.state || "Não informado"
        modal.style.display = "flex"
      }
    })
    closeBtn.onclick = function () {
      modal.style.display = "none"
    }

    window.onclick = function (event) {
      if (event.target === modal) {
        modal.style.display = "none"
      }
    }

  }
}