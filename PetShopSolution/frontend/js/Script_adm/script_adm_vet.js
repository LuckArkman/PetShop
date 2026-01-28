// ===================== MODAIS =====================
const modal_vet = document.getElementById("backdropSimplevet")
const modal_vet_edit = document.getElementById("backdropSimplevetedit")

const form_vet = document.getElementById("form_vet")
const form_vet_edit = document.getElementById("form_vet_edit")

const btn_add_vet = document.getElementById("btn_add_vet")
const btn_cancel_vet = document.getElementById("cancelSimplevet")
const btn_cancel_vet_edit = document.getElementById("cancelSimplevetedit")

const btn_cad_vet = document.getElementById("saveSimplevet")
const btn_save_vet_edit = document.getElementById("saveSimplevetedit")

let VET_ID_EDIT = null

// ===================== LISTAR VETERINÁRIOS =====================
document.addEventListener("DOMContentLoaded", async () => {
    const table = document.querySelector("table")

    try {
        const req = await fetch("https://petrakka.com:7231/api/MedicoVeterinario/MedicoVeterinarios")
        const vets = await req.json()

        vets.forEach(vet => {
            const tr = document.createElement("tr")

            tr.innerHTML = `
                <td>${vet.nome}</td>
                <td>${vet.especialidade}</td>
                <td>${vet.crmv}</td>
                <td class="actions">
                    <button class="btn-edit">
                        <i class="fa-solid fa-pen"></i> Editar
                    </button>
                    <button class="btn-delete">
                        <i class="fa-solid fa-trash"></i> Excluir
                    </button>
                </td>
            `

            const btn_edit = tr.querySelector(".btn-edit")
            const btn_delete = tr.querySelector(".btn-delete")

            // ===== EDITAR =====
            btn_edit.addEventListener("click", () => {
                modal_vet_edit.style.display = "flex"
                VET_ID_EDIT = vet.id
                document.getElementById("fCmrvvetedit").value = vet.crmv || ""
                document.getElementById("fNamevetedit").value = vet.nome || ""
                document.getElementById("fEmailvetedit").value = vet.email || ""
                document.getElementById("fEspvetedit").value = vet.especialidade || ""
                document.getElementById("fTelvetedit").value = vet.telefone || ""
                document.getElementById("fsenhavetedit").value = ""
                document.getElementById("fConsenhavetedit").value = ""
            })

            // ===== DELETAR =====
            btn_delete.addEventListener("click", async () => {
                if (!confirm("Tem certeza que deseja excluir este veterinário?")) return

                try {
                    const req = await fetch(
                        `https://petrakka.com:7231/api/MedicoVeterinario/delete?crmv=${vet.crmv}`,
                        { method: "DELETE" }
                    )
                    const res = await req.json()
                    setTimeout(() => location.reload(), 800)
                } catch {
                    showMessage("Erro interno", "red")
                }
            })

            table.appendChild(tr)
        })

    } catch (error) {
        console.error(error)
    }
})

// ===================== MODAL CRIAR =====================
btn_add_vet.addEventListener("click", () => {
    modal_vet.style.display = "flex"
    modal_vet_edit.style.display = "none"
    form_vet.reset()
})

btn_cancel_vet.addEventListener("click", () => {
    modal_vet.style.display = "none"
})

// ===================== TOAST =====================
function showMessage(message, color = "black", duration = 2000) {
    let toast = document.getElementById("toastMessage")
    toast.textContent = message
    toast.style.color = color
    toast.style.display = "block"
    toast.style.opacity = "1"

    setTimeout(() => {
        toast.style.opacity = "0"
        setTimeout(() => toast.style.display = "none", 300)
    }, duration)
}

function showMessageedit(message, color = "black", duration = 2000) {
    let toast = document.getElementById("toastMessageedit")
    toast.textContent = message
    toast.style.color = color
    toast.style.display = "block"
    toast.style.opacity = "1"

    setTimeout(() => {
        toast.style.opacity = "0"
        setTimeout(() => toast.style.display = "none", 300)
    }, duration)
}

// ===================== CRIAR VETERINÁRIO =====================
btn_cad_vet.addEventListener("click", async (e) => {
    e.preventDefault()

    const data = {
        Nome: document.getElementById("fNamevet").value,
        Email: document.getElementById("fEmailvet").value,
        CRMV: document.getElementById("fCmrv").value,
        Especialidade: document.getElementById("fEspvet").value,
        Telefone: document.getElementById("fTelvet").value,
        Password: document.getElementById("fsenhavet").value,
        ConfirmPassword: document.getElementById("fConsenhavet").value
    }

    if (data.Password !== data.ConfirmPassword) {
        showMessage("Senhas não coincidem", "red")
        return
    }

    try {
        const req = await fetch(
            "https://petrakka.com:7231/api/MedicoVeterinario/register",
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            }
        )

        const res = await req.json()

        if (!req.ok) {
            showMessage(res.message || "Erro ao cadastrar", "red")
            return
        }

        showMessage("Veterinário cadastrado com sucesso", "green")
        setTimeout(() => location.reload(), 800)

    } catch {
        showMessage("Erro interno", "red")
    }
})

// ===================== MODAL EDITAR =====================
btn_cancel_vet_edit.addEventListener("click", () => {
    modal_vet_edit.style.display = "none"
})

// ===================== ATUALIZAR VETERINÁRIO =====================
btn_save_vet_edit.addEventListener("click", async (e) => {
    console.log("clicou")
    const data = {
        id: VET_ID_EDIT,
        nome: document.getElementById("fNamevetedit").value,
        email: document.getElementById("fEmailvetedit").value,
        crmv: document.getElementById("fCmrvvetedit").value,
        especialidade: document.getElementById("fEspvetedit").value,
        telefone: document.getElementById("fTelvetedit").value,
        password: document.getElementById("fsenhavetedit").value,
        confirmPassword: document.getElementById("fConsenhavetedit").value
    }

    if (data.password !== data.confirmPassword) {
        showMessageedit("Senhas não coincidem", "red")
        return
    }

    if (data.password == "") {
        showMessageedit("Digite a senha para atualizar", "red")
        return
    }
    try {
        const req = await fetch(
            "https://petrakka.com:7231/api/MedicoVeterinario/update",
            {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            }
        )
        if (req.status == 204) {
            showMessageedit("Veterinário atualizado com sucesso", "green")
            setTimeout(() => location.reload(), 800)
        } else {
            showMessageedit("Erro ao atualizar", "red")
        }

    } catch (error){
        showMessageedit("Erro interno", "red")
        console.log(error)
    }
})
