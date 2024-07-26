const search = document.querySelector('.input-group input'),
    table_rows = document.querySelectorAll('tbody tr');
table_headings = document.querySelectorAll('thead th');

search.addEventListener('input', searchTable);

function searchTable() {
    console.log('Inicio');
    table_rows.forEach((row, i) => {
        let talbe_data = row.textContent.toLowerCase(),
            search_data = search.value.toLowerCase();

        row.classList.toggle('hide', talbe_data.indexOf(search_data) < 0);
        row.style.setProperty('--delay', i / 25 + 's');

        document.querySelectorAll('tbody tr:not(.hide)').forEach((visible_row, i) => {
            visible_row.style.backgroundColor = (i % 2 == 0) ? 'transparent' : '#0000000b'
        });
    })
}

table_headings.forEach((head, i) => {
    let sort_asc = true;
    head.onclick = () => {
        table_headings.forEach(head => head.classList.remove('active'));
        head.classList.add('active');

        document.querySelectorAll('td').forEach(td => td.classList.remove('active'))
        table_rows.forEach(row => {
            row.querySelectorAll('td')[i].classList.add('active')
        })

        head.classList.toggle('asc', sort_asc);
        sort_asc = head.classList.contains('asc') ? false : true;

        sortTable(i, sort_asc);
    }
})

function sortTable(column, sort_asc) {
    [...table_rows].sort((a, b) => {
        let first_row = a.querySelectorAll('td')[column].textContent.toLowerCase(),
            second_row = b.querySelectorAll('td')[column].textContent.toLowerCase();
        return sort_asc ? (first_row < second_row ? 1 : -1) : (first_row < second_row ? -1 : 1);
    }).map(sorted_row => document.querySelector('tbody').appendChild(sorted_row));


}

///

function handleRowClick(row) {
    var studyInstanceUID = row.getAttribute('data-study-instance-uid');
    // Aquí puedes cambiar el comportamiento si es necesario
    console.log("Row clicked: " + studyInstanceUID);
}

function handleButtonClick(event, studyInstanceUID) {
    event.stopPropagation(); // Para evitar que el click en el botón dispare el click en la fila
    fetch(`/MainListStudies/GetStudyInstances`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': '@Antiforgery.GetTokens(HttpContext).RequestToken'
        },
        body: JSON.stringify(studyInstanceUID)
    }).then(response => {
        if (response.ok) {
            return response.blob();
        } else {
            throw new Error('Error en la solicitud');
        }
    }).then(blob => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = 'dicom_files.zip';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
    }).catch(error => {
        console.error('Error:', error);
    });


}