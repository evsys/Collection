const buttonsDelete = document.querySelectorAll('.deleteCollection');

for (let i = 0; i < buttonsDelete.length; i++) {
    buttonsDelete[i].addEventListener('click', e => {
        document.querySelector('#idCollection').value = e.target.getAttribute("collectionId");
    });
}