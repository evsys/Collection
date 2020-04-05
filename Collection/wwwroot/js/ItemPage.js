document.querySelector('#addTeg').addEventListener('click', e => {
    e.preventDefault();
    let text = document.querySelector('#allTegs').value;
    const value = document.querySelector('#oneTag').value;
    if (value != "") {
        text += '#' + value;
        document.querySelector('#oneTag').value = "";
        document.querySelector('#allTegs').value = text;
    }
});

const buttonsDeleteItems = document.querySelectorAll('.deleteItem');

for (let i = 0; i < buttonsDeleteItems.length; i++) {
    buttonsDeleteItems[i].addEventListener('click', e => {
        document.querySelector('#ItemDelete').value = e.target.getAttribute('idItem');
    });
}

const checkboxes = document.querySelectorAll('input[type=checkBox]');

for (let i = 0; i < checkboxes.length; i++) {
    checkboxes[i].addEventListener('change', (event) => {
        const target = event.target;
        const checked = event.target.checked;
        target.previousElementSibling.value = checked ? 'on' : 'off';
    });
}