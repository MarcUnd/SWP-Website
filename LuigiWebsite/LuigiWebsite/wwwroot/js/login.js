//alert("aljsdkföaljwlf")
//jQuery ... ist eine JS-Bibliothek, sie vereinfacht das Arbeiten mit JS (kürzer, kompatible mit dem meisten Browsern)

$(document).ready(() => {
    //alert("reg-ready")
    $("#email").blur(() => {

        $.ajax({
            url: "/customer/checkEmail", method: "GET", data: { email: $("#email").val() }
        })

            .done((dataFromServer) => {
                //alert("serverurl erreichbar " + dataFromServer)
                if (dataFromServer === false) {
                    $("#ausgabe").css("visibility", "visible");
                    $("#email").addClass("redBorder");

                    document.getElementById("ausgabe").innerHTML = "This email is not in our database! Please register!"
                } else {
                    $("#ausgabe").css("visibility", "hidden");
                    $("#email").removeClass("redBorder");
                }
            })

            .fail(() => {
                alert("serverurl nicht erreichbar")
            });
        //alert("blur")

    });

    $("#btnToggle").click(() => {
        $("#fromReg").toggle(2000);
    })

});







