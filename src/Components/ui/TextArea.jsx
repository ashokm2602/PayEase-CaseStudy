import React from "react";

const Textarea = ({ 
  placeholder = "", 
  value, 
  onChange, 
  rows = 3,
  disabled = false, 
  required = false,
  className = "",
  ...props 
}) => {
  return (
    <textarea
      placeholder={placeholder}
      value={value}
      onChange={onChange}
      rows={rows}
      disabled={disabled}
      required={required}
      className={`
        w-full px-3 py-2 border border-gray-300 rounded-lg text-sm resize-none
        form-input
        disabled:bg-gray-100 disabled:cursor-not-allowed
        placeholder:text-gray-400
        ${className}
      `}
      {...props}
    />
  );
};

export default Textarea;