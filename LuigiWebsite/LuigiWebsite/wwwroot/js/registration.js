//alert("aljsdkföaljwlf")
//jQuery ... ist eine JS-Bibliothek, sie vereinfacht das Arbeiten mit JS (kürzer, kompatible mit dem meisten Browsern)

$(document).ready(() => {
    //alert("reg-ready")
    $("#email").blur(() => {

        $.ajax({
            url: "/customer/CheckEmailAsync",
            method: "GET",
            data: { email: $("#email").val() }
        })
            .done((dataFromServer) => {
                //alert("serverurl erreichbar " + dataFromServer)
                if (dataFromServer === true) {
                    $("#ausgabe").css("visibility", "visible");
                    $("#email").addClass("redBorder");
                    document.getElementById("ausgabe").innerHTML = "Es existiert bereits ein Account mit dieser Email Adresse!";
                } else {
                    $("#ausgabe").css("visibility", "hidden");
                    $("#email").removeClass("redBorder");
                    document.getElementById("ausgabe").innerHTML = "is ogge";

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







