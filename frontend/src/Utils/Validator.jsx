function lengthValidator(maxlength)
{
    return (target_value, original_value) =>
        (target_value.length > maxlength) ? original_value : target_value;
}

function floatValidator(decimal_length)
{
    return (target_value, original_value) => {
        try{
            let nums = target_value.split('.');
            if (nums.length === 1){
                if (target_value.length === 0) return target_value;
                else {
                    let int = parseInt(target_value);
                    if (isNaN(int)) return "";
                    return int;
                }
            } else {
                let int = parseInt(nums[0]);
                let dec = nums[1];
                if (dec.length === 0);
                else if (dec.length < decimal_length) dec = parseInt(dec);
                else {
                    dec = dec.substring(0, decimal_length);
                    dec = parseInt(dec);
                }
                return int + '.' + dec;
            }
        }catch{
            return original_value;
        }
    }
}

export { lengthValidator, floatValidator}