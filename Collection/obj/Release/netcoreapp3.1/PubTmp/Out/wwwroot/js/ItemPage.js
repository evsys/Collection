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