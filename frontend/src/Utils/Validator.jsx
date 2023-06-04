function lengthValidator(maxlength)
{
    return (target_value, original_value) =>
        (target_value.length > maxlength) ? original_value : target_value;
}

function floatValidator(decimal_length)
{
    return (target_value, original_value) => {
        try{
            if (target_value === "") return target_value;
            let nums = target_value.split('.');

            let int = parseInt(nums[0])
            if (isNaN(int)) return original_value;
            
            if (nums.length === 1) return "" + int

            let float = parseFloat("0." + nums[1])
            if (isNaN(float)) return original_value;

            return "" + int + "." + nums[1].substring(0, decimal_length);
        }catch{
            return original_value;
        }
    }
}

export { lengthValidator, floatValidator}