import React from "react";

const Input = ({ 
  type = "text", 
  placeholder = "", 
  value, 
  onChange, 
  disabled = false, 
  required = false,
  className = "",
  ...props 
}) => {
  return (
    <input
      type={type}
      placeholder={placeholder}
      value={value}
      onChange={onChange}
      disabled={disabled}
      required={required}
      className={`
        w-full px-3 py-2 border border-gray-300 rounded-lg text-sm
        form-input
        disabled:bg-gray-100 disabled:cursor-not-allowed
        placeholder:text-gray-400
        ${className}
      `}
      {...props}
    />
  );
};

export default Input;