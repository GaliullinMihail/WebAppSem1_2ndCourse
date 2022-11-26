let  form = document.querySelector('.js-form'),
    formInputs = document.querySelectorAll('.js-input'),
    inputLogin = document.querySelector('.js-login'),
    inputPassword = document.querySelector('.js-password'),
    inputPasswordRepeat= document.querySelector('.js-password-repeat'),
    loginHelper = document.querySelector('.hideInput'),
    passwordHelper = document.querySelector('.hidePassword'),
    repeatPasswordHelper = document.querySelector('.hidePasswordRepeat');

function validateLogin(login){
    let re = /^[a-zA-Z0-9]+$/;
    return login.length<= 30 && login.length > 5 && re.test(String(login).toLowerCase());
}

function validatePassword(password){
    let re = /^[a-zA-Z0-9]+$/;
    return password.length <= 30 && password.length > 5 && re.test(String(password).toLowerCase());;
}

form.onsubmit = function ()
{
    let emptyInputs = Array.from(formInputs).filter(input => input.value === ''),
        loginVal = inputLogin.value,
        passwordVal = inputPassword.value,
        passwordRepeatVal = inputPasswordRepeat.value;
    formInputs.forEach(function(input){
        if(input.value === '') {
            input.classList.add('error'); 
        } else {
            input.classList.remove('error');
        }
    });
     
    if (emptyInputs.length != 0){
        return false;
    }
    
    if(!validateLogin(loginVal)) {
        inputLogin.classList.add('error');
        loginHelper.style.display='block';
        return false;
    } else {
        loginHelper.style.display='none';
        inputLogin.classList.remove('error');
    }
    
    if(passwordVal != passwordRepeatVal){
        repeatPasswordHelper.style.display ='block';
        inputPasswordRepeat.classList.add('error');
        return false;
    } else {
        repeatPasswordHelper.style.display='none';
        inputPasswordRepeat.classList.remove('error');
    }
    
    if(!validatePassword(passwordVal)){
        passwordHelper.style.display = 'block';
        inputPassword.classList.add('error');
        return false;
    }
    else {
        passwordHelper.style.display = 'none';
        inputPassword.classList.remove('error');
    }
    
}