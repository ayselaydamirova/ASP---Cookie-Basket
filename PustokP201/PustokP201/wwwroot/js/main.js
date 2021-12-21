$(function () {

    $(document).on("click", ".single-eye-btn", function () {

        let id = $(this).attr("data-id");

        fetch(`/home/getbook/${id}`)
            //.then(response => response.json())
            .then(response => response.text())
            .then(data => {

                console.log(data)
                $("#detailModal .modal-content").html(data)
              /*  $("#detailModal").find(".product-title").text(data.name)*/
            })
            
      

        $("#detailModal").modal("show")
    })

    $(document).on("click", ".add-basket", function (e) {
        e.preventDefault();


        let url = $(this).attr("href");
        fetch(url)
            .then(response => {
                console.log(response)
                if (response.ok) {
                    alert("Kitab sebete elave edildi!")
                }
                else {
                    alert("Xeta bas verdi!")
                }
            })

        console.log(url)
    })


})