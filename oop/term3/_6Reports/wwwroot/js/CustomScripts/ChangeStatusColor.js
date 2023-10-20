
function ChangeColor(){
    let elements = document.querySelectorAll('input.form-control');
    for(let el of elements){
        if (el.getAttribute("id") === "task_Status"){
            if (el.getAttribute("value") === "Open"){
                el.setAttribute("style", "color:green; font-weight: bold");
            }
            else if (el.getAttribute("value") === "Active"){
                el.setAttribute("style", "color:orange; font-weight: bold");
            }
            else if (el.getAttribute("value") === "Resolved"){
                el.setAttribute("style", "color:red; font-weight: bold");
            }
        }
    }
}
ChangeColor();