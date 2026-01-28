// ===================== MODAIS =====================
const modal_user = document.getElementById("backdropSimpleuser")
const modal_user_edit = document.getElementById("backdropSimpleuseredit")

const form_user = document.getElementById("form_user")
const form_user_edit = document.getElementById("form_user_edit")

const btn_add_user = document.getElementById("btn_add_user")
const btn_cancel_user = document.getElementById("cancelSimpleuser")
const btn_cancel_user_edit = document.getElementById("cancelSimpleuseredit")

const btn_cad = document.getElementById("saveSimpleuser")
const btn_save_user_edit = document.getElementById("saveSimpleuseredit")

let USER_ID_EDIT = null
// ===================== MOSTRAR USERS =====================
document.addEventListener("DOMContentLoaded", async () => {
    const content_user_table = document.getElementById("content_user")

    try {
        const req_users = await fetch("https://petrakka.com:7231/api/Responsavel/Responsaveis")
        const res_user = await req_users.json()

        res_user.forEach(user => {
            const tr = document.createElement("tr")

            const td_nome = document.createElement("td")
            const td_email = document.createElement("td")
            const td_rg = document.createElement("td")
            const td_actions = document.createElement("td")

            td_actions.classList.add("actions")

            td_nome.textContent = user.firstName
            td_email.textContent = user.email
            td_rg.textContent = user.rg

            // ===== BOTÃO EDITAR =====
            const btn_edit = document.createElement("button")
            btn_edit.classList.add("btn-edit")
            btn_edit.innerHTML = `<i class="fa-solid fa-pen"></i> Editar`
            btn_edit.dataset.id = user.id

            // ===== BOTÃO DELETE =====
            const btn_delete = document.createElement("button")
            btn_delete.classList.add("btn-delete")
            btn_delete.innerHTML = `<i class="fa-solid fa-trash"></i> Excluir`
            btn_delete.dataset.id = user.id

            // ===== EVENTO EDITAR =====
            btn_edit.addEventListener("click", () => {
                modal_user_edit.style.display = "flex"
                USER_ID_EDIT = user.id
                document.getElementById("fNameuseredit").value = user.firstName || ""
                document.getElementById("fEmailuseredit").value = user.email || ""
                document.getElementById("fRguseredit").value = user.rg || ""
                document.getElementById("fCpfuseredit").value = user.cpf || ""
                document.getElementById("fsenhauseredit").value = ""
                document.getElementById("fConsenhauseredit").value = ""
            })

            // ===== EVENTO DELETE =====
            btn_delete.addEventListener("click", async () => {
                if (!confirm("Tem certeza que deseja excluir este usuário?")) return

                try {
                    const req = await fetch(`https://petrakka.com:7231/api/Responsavel/delete?mail=${user.email}`, {
                        method: "DELETE",
                        headers: { "Content-Type": "application/json" }
                    })
                    const res = await req.json()
                    console.log(res)
                    if (!req.ok) {
                        showMessage("Erro ao excluir usuário", "red")
                        return
                    }

                    showMessage("Usuário excluído com sucesso", "green")
                    setTimeout(() => location.reload(), 800)

                } catch {
                    showMessage("Erro interno", "red")
                }
            })

            td_actions.appendChild(btn_edit)
            td_actions.appendChild(btn_delete)

            tr.appendChild(td_nome)
            tr.appendChild(td_email)
            tr.appendChild(td_rg)
            tr.appendChild(td_actions)

            content_user_table.appendChild(tr)
        })

    } catch (error) {
        console.log(error)
    }
})


// ===================== MODAL CRIAR USER =====================

btn_add_user.addEventListener("click", () => {
    modal_user.style.display = "flex"
    modal_user_edit.style.display = "none"
    form_user.reset()
})

btn_cancel_user.addEventListener("click", () => {
    modal_user.style.display = "none"
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


// ===================== CRIAR USER =====================
btn_cad.addEventListener("click", async (e) => {
    e.preventDefault()

    const data = {
        Email: document.getElementById("fEmailuser").value,
        Password: document.getElementById("fsenhauser").value,
        ConfirmPassword: document.getElementById("fConsenhauser").value,
        FirstName: document.getElementById("fNameuser").value,
        RG: document.getElementById("fRguser").value,
        Cpf: document.getElementById("fCpfuser").value
    }

    if (data.Password !== data.ConfirmPassword) {
        showMessage("Senhas não coincidem", "red")
        return
    }

    try {
        const req = await fetch("https://petrakka.com:7231/api/Responsavel/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        })

        const res = await req.json()

        if (!req.ok) {
            showMessage(res.message || "Erro ao cadastrar", "red")
            return
        }

        showMessage("Cadastro realizado com sucesso", "green")
        setTimeout(() => location.reload(), 800)

    } catch {
        showMessage("Erro interno", "red")
    }
})


// ===================== MODAL EDITAR =====================

btn_cancel_user_edit.addEventListener("click", () => {
    modal_user_edit.style.display = "none"
})


// ===================== ATUALIZAR USER =====================
btn_save_user_edit.addEventListener("click", async (e) => {
    e.preventDefault()
    let toastMessageedit = document.getElementById("toastMessageedit")
    const data = {
        id: USER_ID_EDIT,
        firstName: document.getElementById("fNameuseredit").value,
        email: document.getElementById("fEmailuseredit").value,
        rg: document.getElementById("fRguseredit").value,
        cpf: document.getElementById("fCpfuseredit").value,
        password: document.getElementById("fsenhauseredit").value,
        confirmPassword: document.getElementById("fConsenhauseredit").value,
    }

    if (data.password !== data.confirmPassword) {
        showMessage("Senhas não coincidem", "red")
        return
    }

    if(data.password === ""){
        showMessage("Digite a senha para alterar os dados","red")
        return
    }

    try {
        const req = await fetch("https://petrakka.com:7231/api/Responsavel/update", {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        })
        const res = await req.json()
        console.log(res)
        if(res.user){
        showMessage("Atualizado com sucesso","green")
        setTimeout(() => location.reload(),2000)
        }else{
            showMessage(res.message,"red")
        }
    } catch {
        showMessage("Erro interno","red")
    }
})
