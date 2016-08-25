
      $(document).ready(function() {
          $('#dsdinhdang').DataTable({
              responsive: true,
              "language": {
                  "lengthMenu": "Hiển thị _MENU_ dòng dữ liệu trên một trang",
                  "info": "Hiển thị _START_ trong tổng số _TOTAL_ dòng dữ liệu",
                  "infoEmpty": "Dữ liệu rỗng",
                  "emptyTable": "Chưa có dữ liệu nào",

                  "Proccessing": "Đang sử lý...",
                  "search": "Tìm kiếm",
                  "loadingRecords": "không tìm thấy dữ liệu",
                  "infoFiltered": "(Được từ tổng số _MAX_ dòng dữ liệu)",
                  "paginate": {
                      "first": "|<",
                      "last": ">|",
                      "next": ">>",
                      "previous": "<<"
                  }
              },
              //"lengthMenu": [[-1, 50, 40, 30, 20, 15, 10, 5, 3], ["Tất cả", 50, 40, 30, 20, 15, 10, 5, 3]]
              "lengthMenu": [[5, 10, 20, 30, 40, 50, -1], [5, 10, 20, 30, 40, 50, "Tat Ca"]]
          });
      });
