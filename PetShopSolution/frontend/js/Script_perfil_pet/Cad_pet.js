const btn_add_pet = document.getElementById("btn_add_pet")
const btn_remove_pet = document.getElementById("btn_remove_pet")
const backdropSimple = document.getElementById("backdropSimple")
const cancelSimple = document.getElementById("cancelSimple")
const saveSimple = document.getElementById("saveSimple")
const select_pet = document.getElementById("select_pet")
const div_msg = document.getElementById("div_msg")
const info_bottom_pet = document.getElementById("info_bottom_pet")

btn_add_pet.addEventListener("click", () => backdropSimple.style.display = "flex")
cancelSimple.addEventListener("click", () => backdropSimple.style.display = "none")
document.querySelectorAll(".modal-backdrop").forEach(b =>
    b.addEventListener("click", e => { if (e.target === b) b.style.display = "none" })
)

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
    div_msg.textContent = "Usuário não autenticado!"
    div_msg.style.color = "red"
    throw new Error("Usuário não autenticado")
}
const payload = getPayloadFromToken(token)
const userId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]

function updatePetUI(pet) {
    document.getElementById("animal_name").textContent = pet.Nome
    document.getElementById("race_animal").textContent = pet.Raca || "Sem Raça"
    document.getElementById("petSpecies").textContent = pet.Especie
    document.getElementById("petAge").textContent = pet.Idade
    document.getElementById("petWeight").textContent = `${pet.Peso}kg`
    document.getElementById("petSex").textContent = pet.Sexo
    document.getElementById("petPorte").textContent = pet.Porte
    document.getElementById("petCastrated").textContent = pet.Castrado ? "Sim" : "Não"
    const avatar = document.getElementById("avatar")
    avatar.style.background = pet.Sexo === "Fêmea" ? "Pink" : "linear-gradient(135deg,#3b82f6,#60a5fa)"
}

async function populatePets() {
    select_pet.innerHTML = `<option value="">Pet Atual</option>`
        try {
            const res = await fetch(`http://localhost:5280/api/Animal/animais`, {
               method:"GET"
            })
            const pets = await res.json()
            pets.forEach(pet => {
                const option = document.createElement("option")
                option.value = pet.id
                option.textContent = pet.Nome
                select_pet.appendChild(option)
            })
        } catch (err) {
            console.error("Erro ao buscar pet:", err)
        }
}

select_pet.addEventListener("change", async () => {
    const selectedId = select_pet.value
    if (!selectedId) return

    try {
        const res = await fetch(`http://localhost:5280/api/Animal/animal?animal=${selectedId}`, {
            headers: { "Authorization": `Bearer ${token}` }
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
    } catch (err) {
        console.error("Erro ao buscar pet selecionado:", err)
    }
})

saveSimple.addEventListener("click", async () => {
    const fName = document.getElementById("fName").value
    const fSpecies = document.getElementById("fSpecies").value
    const fBreed = document.getElementById("fBreed").value
    const fAge = document.getElementById("fAge").value
    const fWeight = document.getElementById("fWeight").value
    const fPorte = document.getElementById("fPorte").value
    const fSex = document.getElementById("fSex").value
    const fCastrado = document.getElementById("fCastrado").value
    const value_fCastrado = fCastrado === "Sim"

    const token = localStorage.getItem("token")
    if (!token) {
        div_msg.textContent = "Usuário não autenticado!"
        div_msg.style.color = "red"
        return
    }
    const payload = getPayloadFromToken(token)
    const userId = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]

    if (!fName || !fSpecies || !fBreed || !fAge || !fWeight || !fPorte || !fSex || !fCastrado) {
        div_msg.textContent = "Preencha todos os campos!"
        div_msg.style.color = "red"
        setTimeout(() => { div_msg.textContent = "" }, 2000)
        return
    }

    try {
        const req = await fetch("http://localhost:5280/api/Animal/register", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                Nome: fName,
                Especie: fSpecies,
                Raca: fBreed,
                Sexo: fSex,
                Castrado: value_fCastrado,
                Idade: fAge,
                Peso: fWeight,
                Porte: fPorte,
                responsaveis: [userId]
            })
        })

        const res = await req.json()
        if (res.id) {
            let animalIds = JSON.parse(localStorage.getItem("userAnimalIds")) || []
            animalIds.push(res.id)
            localStorage.setItem("userAnimalIds", JSON.stringify(animalIds))

            div_msg.textContent = "Cadastro realizado com sucesso!"
            div_msg.style.color = "green"
            setTimeout(() => { div_msg.textContent = "" }, 2000)

            backdropSimple.style.display = "none"
            await populatePets()
            select_pet.value = res.id
            updatePetUI(res)
            info_bottom_pet.style.display = "block"
        } else {
            div_msg.textContent = "Erro ao cadastrar pet!"
            div_msg.style.color = "red"
            console.log(res)
        }
    } catch (err) {
        div_msg.textContent = "Erro interno!"
        div_msg.style.color = "red"
        console.error(err)
    }
})

// Rota excluir pet
btn_remove_pet.addEventListener("click",async()=>{
    try {
        const req = await fetch(`http://localhost:5280/api/Animal/delete?id=${select_pet.value}`,{
            method:"DELETE"
        })
        const res =  await req.json()
        if(res.Nome){
            let animalIds = JSON.parse(localStorage.getItem("userAnimalIds")) || []
            animalIds = animalIds.filter(id => id !== select_pet.value)
            localStorage.setItem("userAnimalIds", JSON.stringify(animalIds))

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

            div_msg.textContent = "Pet excluído com sucesso!"
            div_msg.style.color = "green"
            setTimeout(() => { div_msg.textContent = "" }, 2000)
            
        }else{
            div_msg.textContent = "Erro ao deletar"
            div_msg.style.color = "red"
            setTimeout(() => { div_msg.textContent = "" }, 2000)
        }
        
    } catch (error) {
        div_msg.textContent = "Erro interno"
        div_msg.style.color = "red"
        setTimeout(() => { div_msg.textContent = "" }, 2000)
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

    const res = await fetch(`http://localhost:5280/api/Animal/animal?animal=${petId}`, {
        method:"GET",
        headers: { "Authorization": `Bearer ${token}` }
    })
    const pet = await res.json()

    document.getElementById("updateName").value = pet.Nome
    document.getElementById("updateSpecies").value = pet.Especie
    document.getElementById("updateBreed").value = pet.Raca
    document.getElementById("updateAge").value = pet.Idade
    document.getElementById("updateWeight").value = pet.Peso
    document.getElementById("updatePorte").value = pet.Porte
    document.getElementById("updateSex").value = pet.Sexo
    document.getElementById("updateCastrado").value = pet.Castrado ? "Sim" : "Não"

    backdropUpdate.style.display = "flex"
})

cancelUpdate.addEventListener("click", () => {
    backdropUpdate.style.display = "none"
})

saveUpdate.addEventListener("click", async (e) => {
    e.preventDefault()
    const petId = select_pet.value

    const updatedPet = {
        id: petId,
        Nome: document.getElementById("updateName").value,
        Especie: document.getElementById("updateSpecies").value,
        Raca: document.getElementById("updateBreed").value,
        Idade: document.getElementById("updateAge").value,
        Peso: document.getElementById("updateWeight").value,
        Porte: document.getElementById("updatePorte").value,
        Sexo: document.getElementById("updateSex").value,
        Castrado: document.getElementById("updateCastrado").value === "Sim",
        responsaveis: [userId]
    }

    const req = await fetch(`http://localhost:5280/api/Animal/update?id=${petId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(updatedPet)
    })
    const res = await req.json()
    if (res.id) {
        updatePetUI(res)
        backdropUpdate.style.display = "none"
        div_msg.textContent = "Pet atualizado com sucesso!"
        div_msg.style.color = "green"
        setTimeout(() => { div_msg.textContent = "" }, 2000)
        const optionToUpdate = Array.from(select_pet.options).find(opt => opt.value === res.id)
        if (optionToUpdate) {
            optionToUpdate.textContent = res.Nome
        }
    } else {
        div_msg.textContent = "Erro ao atualizar pet!"
        div_msg.style.color = "red"
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
const closeVacinasView = document.getElementById("closeVacinasView")
const listaVacinasView = document.getElementById("lista_vacinas_view")

let vacinas = []

document.getElementById("btn_add_vacina").addEventListener("click", () => {
    backdropVacina.style.display = "flex"
})

document.getElementById("cancelVacina").addEventListener("click", () => {
    backdropVacina.style.display = "none"
})

saveVacina.addEventListener("click", () => {
    const nome = document.getElementById("vacinaNome").value.trim()
    const data = document.getElementById("vacinaData").value
    const vet = document.getElementById("vacinaVet").value.trim()

    if (!nome || !data) {
        alert("Preencha pelo menos o nome da vacina e a data.")
        return
    }

    vacinas.push({ nome, data, vet })
    atualizarContador(vacinaCount, vacinas.length)
    document.getElementById("vacinaNome").value = ""
    document.getElementById("vacinaData").value = ""
    document.getElementById("vacinaVet").value = ""
    backdropVacina.style.display = "none"
})

toggleVacinas.addEventListener("click", () => {
    renderVacinas()
    backdropVacinasView.style.display = "flex"
})

// fechar modal de visualização
closeVacinasView.addEventListener("click", () => {
    backdropVacinasView.style.display = "none"
})

function atualizarContador() {
    vacinaCount.textContent = `(${vacinas.length})`
}

function renderVacinas() {
    listaVacinasView.innerHTML = ""
    if (vacinas.length === 0) {
        listaVacinasView.innerHTML = "<li>Nenhuma vacina cadastrada</li>"
        listaVacinasView.style.listStyleType = "none"
        return
    }

    vacinas.forEach((v, i) => {
        const li = document.createElement("li")
        li.classList.add("vacina_item")
        li.innerHTML = `
            <div>
                <strong>${v.nome}</strong> - ${new Date(v.data).toLocaleDateString()}<br>
                <small>${v.vet || "—"}</small>
            </div>
            <button class="btnExcluir" data-index="${i}">Excluir</button>
        `
        listaVacinasView.appendChild(li)
    })

    document.querySelectorAll(".btnExcluir").forEach(btn => {
        btn.addEventListener("click", e => {
            const idx = e.target.dataset.index
            vacinas.splice(idx, 1)
            atualizarContador()
            renderVacinas()
        })
    })
}

/*Fim modal mostrar vacina*/
const toggleObs = document.getElementById("toggleObs")
const obsCount = document.getElementById("obsCount")
const saveObs = document.getElementById("saveObs")
const backdropObs = document.getElementById("backdropObs")
const backdropObsView = document.getElementById("backdropObsMedView")
const closeObsView = document.getElementById("closeObsMedView")
const listaObsView = document.getElementById("lista_obs_med")

let obsMed = []

document.getElementById("btn_add_obs").addEventListener("click", () => backdropObs.style.display = "flex")
document.getElementById("cancelObs").addEventListener("click", () => backdropObs.style.display = "none")

saveObs.addEventListener("click", () => {
    const descricao = document.getElementById("obsDescricao").value.trim()
    const data = document.getElementById("obsData").value

    if (!descricao || !data) { alert("Preencha todos os campos."); return }

    obsMed.push({ descricao, data })
    atualizarContador(obsCount, obsMed.length)
    document.getElementById("obsDescricao").value = ""
    document.getElementById("obsData").value = ""
    backdropObs.style.display = "none"
})

toggleObs.addEventListener("click", () => {
    renderList(listaObsView, obsMed, "Observação")
    backdropObsView.style.display = "flex"
})

closeObsView.addEventListener("click", () => backdropObsView.style.display = "none")

// Modal Contato
const toggleContato = document.getElementById("toggleContat")
const contatoCount = document.getElementById("contatoCount")
const saveContato = document.getElementById("saveContato")
const backdropContato = document.getElementById("backdropContato")
const backdropContatoView = document.getElementById("backdropContatoView")
const closeContatoView = document.getElementById("closeContatoView")
const listaContatoView = document.getElementById("lista_contato")

let contatos = []

document.getElementById("btn_add_contato").addEventListener("click", () => backdropContato.style.display = "flex")
document.getElementById("cancelContato").addEventListener("click", () => backdropContato.style.display = "none")

saveContato.addEventListener("click", () => {
    const nome = document.getElementById("contatoNome").value.trim()
    const telefone = document.getElementById("contatoTelefone").value.trim()

    if (!nome || !telefone) { alert("Preencha todos os campos.")
        return 
    }

    contatos.push({ nome, telefone })
    atualizarContador(contatoCount, contatos.length)
    document.getElementById("contatoNome").value = ""
    document.getElementById("contatoTelefone").value = ""
    backdropContato.style.display = "none"
})

toggleContato.addEventListener("click", () => {
    renderList(listaContatoView, contatos, "Contato")
    backdropContatoView.style.display = "flex"
})

closeContatoView.addEventListener("click", () => backdropContatoView.style.display = "none")

// === Funções auxiliares ===
function atualizarContador(element, count) {
    element.textContent = `(${count})`
}

function renderList(container, arr, tipo) {
    container.innerHTML = ""
    if (arr.length === 0) {
        const li = document.createElement("li")
        li.textContent = `Nenhum ${tipo.toLowerCase()} cadastrado`
        li.style.listStyleType = "none"
        container.appendChild(li)
        return
    }

    arr.forEach((item, i) => {
        const li = document.createElement("li")
        li.classList.add("vacina_item")
        li.style.listStyleType = "none"
        let content = ""
        if (tipo === "Vacina") {
            content = `<strong>${item.nome}</strong> - ${new Date(item.data).toLocaleDateString()}<br><small>${item.vet || "—"}</small>`
        } else if (tipo === "Observação") {
            content = `<strong>${new Date(item.data).toLocaleDateString()}</strong> - ${item.descricao}`
        } else if (tipo === "Contato") {
            content = `<strong>${item.nome}</strong> - ${item.telefone}`
        }
        li.innerHTML = `<div>${content}</div><button class="btnExcluir" data-index="${i}">Excluir</button>`
        container.appendChild(li)
    })

    container.querySelectorAll(".btnExcluir").forEach(btn =>{
        btn.addEventListener("click", e => {
            const idx = e.target.dataset.index
            arr.splice(idx, 1)
            atualizarContador(
                tipo === "Vacina" ? vacinaCount :
                tipo === "Observação" ? obsCount : contatoCount,
                arr.length
            )
            renderList(container, arr, tipo)
        })
    })
}